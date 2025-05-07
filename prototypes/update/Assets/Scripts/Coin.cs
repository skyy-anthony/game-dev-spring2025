using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CoinCollector.Instance.AddCoin(1);
            Destroy(gameObject);
        }
    }
}
