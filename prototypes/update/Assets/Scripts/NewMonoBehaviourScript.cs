using UnityEngine;

public class DropZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PickableDroppable pd = other.GetComponent<PickableDroppable>();
        if (pd != null && pd.canBeDroppedForPoints)
        {
            ScoreManager.Instance.AddPoints(pd.pointValue);
            Debug.Log("Scored! + " + pd.pointValue);
            Destroy(other.gameObject); // Or disable it
        }
    }
}
