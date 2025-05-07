using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinCollector : MonoBehaviour
{
    public static CoinCollector Instance;

    public int coinCount = 0;
    public TextMeshProUGUI coinText;  // Or use UnityEngine.UI.Text if you're not using TextMeshPro

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        UpdateUI();
    }

    public void AddCoin(int amount)
    {
        coinCount += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (coinText != null)
        {
            coinText.text = "Coins: " + coinCount;
        }
    }
}
