using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    public static ScoreUI Instance;

    public TextMeshProUGUI scoreText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void UpdateScoreText(int score)
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score.ToString();
    }
}
