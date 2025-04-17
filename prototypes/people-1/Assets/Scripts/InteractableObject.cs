using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public Transform player;
    public float visibilityDistance = 5f;

    private Renderer objectRenderer;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();

        if (objectRenderer == null)
        {
            // Check children if renderer is not on the root
            objectRenderer = GetComponentInChildren<Renderer>();
        }

        if (player == null)
        {
            Debug.LogWarning("Player Transform is not assigned.");
        }

        if (objectRenderer == null)
        {
            Debug.LogError("No Renderer found on this object or its children.");
        }
    }

    void Update()
    {
        if (player == null || objectRenderer == null) return;

        float distance = Vector3.Distance(player.position, transform.position);
       // Debug.Log($"Distance to player: {distance}");

        if (distance <= visibilityDistance)
        {
            SetVisibility(true);
        }
        else
        {
            SetVisibility(false);
        }
    }

    private void SetVisibility(bool visible)
    {
        if (objectRenderer.enabled != visible)
        {
            objectRenderer.enabled = visible;
            Debug.Log($"Set visibility to: {visible}");
        }
    }

}
