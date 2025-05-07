using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public Transform playerBody;
    public float roatationSpeed = 5f; // Not used in this script, but can be useful for other purposes
    public float mouseSensitivity = 100f;

    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Hide and lock cursor
    }

    void Update()
    {
        // Mouse movement input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Vertical camera rotation (up/down)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Prevent over-rotation

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotate player body left/right
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
