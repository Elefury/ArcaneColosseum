using UnityEngine;
using TMPro;

public class ResultDisplay : MonoBehaviour
{
    [Header("Text References")]
    [SerializeField] private TextMeshProUGUI[] lastResultTexts; // 5 текстовых полей
    [SerializeField] private TextMeshProUGUI bestResultText;

    private void Start()
    {
        UpdateResultsDisplay();
    }

    //private void OnEnable()
    //{
    //    UpdateResultsDisplay(); // Обновляем при каждом включении Canvas
    //}

    public void ForceUpdateResults()
    {
        UpdateResultsDisplay();
    }

    public void UpdateResultsDisplay()
    {
        // Получаем данные из ResultManager
        var lastResults = ResultManager.Instance.GetLastResults();
        int bestResult = ResultManager.Instance.GetBestResult();

        // Обновляем лучший результат
        bestResultText.text = bestResult.ToString();

        // Обновляем последние результаты
        for (int i = 0; i < lastResultTexts.Length; i++)
        {
            if (i < lastResults.Count)
            {
                lastResultTexts[i].text = lastResults[i].ToString();
            }
            else
            {
                lastResultTexts[i].text = "0"; // или оставить пустым
            }
        }
    }
}