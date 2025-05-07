using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int score = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddPoints(int points)
    {
        score += points;
        ScoreUI.Instance.UpdateScoreText(score);
    }
}
