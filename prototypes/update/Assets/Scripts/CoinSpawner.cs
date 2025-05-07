using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab;
    public int amount = 10;
    public Vector3 startPos = new Vector3(0, 1, 0);
    public float spacing = 2f;

    void Start()
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 pos = startPos + new Vector3(i * spacing, 0, 0);
            Instantiate(coinPrefab, pos, Quaternion.identity);
        }
    }
}
