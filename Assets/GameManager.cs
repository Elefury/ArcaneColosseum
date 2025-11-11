using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Добавляем пространство имён для TextMeshPro
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using UnityEngine.XR;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Статическая переменная для Singleton
    public Transform player;
    public Transform lobby;
    public XRRayInteractor m_RayInteractor_left;
    public XRRayInteractor m_RayInteractor_right;

    public ObjectSpawner objectSpawner;

    public float gameTime = 180f; // Время игры в секундах (3 минуты)
    private float currentTime; // Текущее оставшееся время

    private int score = 0; // Текущие очки игрока
    public TextMeshProUGUI[] timerTexts; // Массив TextMeshProUGUI для отображения времени
    public TextMeshProUGUI[] scoreTexts; // Массив TextMeshProUGUI для отображения очков

    private bool isGameActive = true; // Флаг активности игры

    private void Awake()
    {
        // Реализация Singleton
        if (instance == null)
        {
            instance = this; // Делаем этот объект единственным экземпляром
        }
        else
        {
            Destroy(gameObject); // Уничтожаем дубликат
        }
    }

   // private void Start()
   // {
   //     currentTime = gameTime; // Устанавливаем начальное время
   //     UpdateTimerDisplay(); // Обновляем отображение таймера
   //     UpdateScoreDisplay(); // Обновляем отображение очков
   // }

   private void OnEnable()
   {
        ResetGame(); // Сбрасываем состояние игры при активации
   }

    public void ResetGame()
    {
        currentTime = gameTime; // Сбрасываем таймер
        score = 0; // Сбрасываем очки
        isGameActive = true; // Активируем игру
        if (objectSpawner != null)
        {
            objectSpawner.gameObject.SetActive(true);
            objectSpawner.ResetSpawner();
        }
        UpdateTimerDisplay(); // Обновляем отображение таймера
        UpdateScoreDisplay(); // Обновляем отображение очков
    }

    private void Update()
    {
        if (isGameActive)
        {
            // Обновляем таймер
            currentTime -= Time.deltaTime;
            UpdateTimerDisplay();

            // Проверяем, закончилось ли время
            if (currentTime <= 0)
            {
                currentTime = 0;
                isGameActive = false;
                EndGame();
            }
            UpdateTimerDisplay();
        }
    }



    // Метод для добавления очков
    public void AddScore(int points)
    {
        if (isGameActive)
        {
            score += points;
            UpdateScoreDisplay();
        }
    }

    // Метод для обновления отображения таймера на всех Canvas
    private void UpdateTimerDisplay()
    {
        if (timerTexts != null && timerTexts.Length > 0)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60f);
            int seconds = Mathf.FloorToInt(currentTime % 60f);
            string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);

            // Обновляем все текстовые элементы
            foreach (TextMeshProUGUI timerText in timerTexts)
            {
                if (timerText != null)
                {
                    timerText.text = "" + timeString;
                }
            }
        }
    }

    // Метод для обновления отображения очков на всех Canvas
    private void UpdateScoreDisplay()
    {
        if (scoreTexts != null && scoreTexts.Length > 0)
        {
            // Обновляем все текстовые элементы
            foreach (TextMeshProUGUI scoreText in scoreTexts)
            {
                if (scoreText != null)
                {
                    scoreText.text = "" + score;
                }
            }
        }
    }

   

    // Метод для завершения игры
    private void EndGame()
    {
        Debug.Log("Игра окончена! Ваши очки: " + score);
        player.position = lobby.position;
        m_RayInteractor_left.gameObject.SetActive(true);
        m_RayInteractor_right.gameObject.SetActive(true);
        // Здесь можно добавить логику завершения игры (например, показать экран результатов)
        objectSpawner.gameObject.SetActive(false);
        List<GameObject> objectsToDestroy = new List<GameObject>();

        // Поиск объектов с каждым тегом и добавление их в список
        objectsToDestroy.AddRange(GameObject.FindGameObjectsWithTag("Any_Vulnerable"));
        objectsToDestroy.AddRange(GameObject.FindGameObjectsWithTag("Ice_Vulnerable"));
        objectsToDestroy.AddRange(GameObject.FindGameObjectsWithTag("Fire_Vulnerable"));
        objectsToDestroy.AddRange(GameObject.FindGameObjectsWithTag("Earth_Vulnerable"));
        objectsToDestroy.AddRange(GameObject.FindGameObjectsWithTag("Radiant_Vulnerable"));

        // Уничтожение всех объектов из списка
        foreach (GameObject obj in objectsToDestroy)
        {
            Destroy(obj);
        }
        ResultManager.Instance.SaveResult(score);
        FindObjectOfType<ResultDisplay>()?.ForceUpdateResults();
        gameObject.SetActive(false);

    }
}