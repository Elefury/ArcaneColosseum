using System.Collections.Generic;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    public static ResultManager Instance;

    private const string BEST_SCORE_KEY = "BestScore";
    private const string LAST_SCORES_KEY = "LastScores";
    private const int MAX_RESULTS = 5;

    private List<int> lastScores = new List<int>();
    private int bestScore;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadResults();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveResult(int score)
    {
        // Обновляем последние результаты
        lastScores.Insert(0, score);
        if (lastScores.Count > MAX_RESULTS)
        {
            lastScores = lastScores.GetRange(0, MAX_RESULTS);
        }

        // Обновляем лучший результат
        if (score > bestScore)
        {
            bestScore = score;
        }

        SaveResults();
    }

    public List<int> GetLastResults()
    {
        return lastScores;
    }

    public int GetBestResult()
    {
        return bestScore;
    }

    private void LoadResults()
    {
        // Загрузка лучшего результата
        bestScore = PlayerPrefs.GetInt(BEST_SCORE_KEY, 0);

        // Загрузка последних результатов
        lastScores.Clear();
        string savedScores = PlayerPrefs.GetString(LAST_SCORES_KEY, "");
        if (!string.IsNullOrEmpty(savedScores))
        {
            string[] scoresArray = savedScores.Split(',');
            foreach (string score in scoresArray)
            {
                if (int.TryParse(score, out int parsedScore))
                {
                    lastScores.Add(parsedScore);
                }
            }
        }
    }

    private void SaveResults()
    {
        // Сохранение лучшего результата
        PlayerPrefs.SetInt(BEST_SCORE_KEY, bestScore);

        // Сохранение последних результатов
        List<string> scoresToSave = new List<string>();
        foreach (int score in lastScores)
        {
            scoresToSave.Add(score.ToString());
        }
        PlayerPrefs.SetString(LAST_SCORES_KEY, string.Join(",", scoresToSave));

        PlayerPrefs.Save();
    }

    public void ClearAllResults()
    {
        // Сбрасываем данные в памяти
        bestScore = 0;
        lastScores.Clear();
    
        // Очищаем PlayerPrefs
        PlayerPrefs.DeleteKey(BEST_SCORE_KEY);
        PlayerPrefs.DeleteKey(LAST_SCORES_KEY);
        PlayerPrefs.Save();
    }
}
