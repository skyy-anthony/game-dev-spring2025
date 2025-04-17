using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class BoyScript : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 700f;

    public GameObject DIalogPanel; // Reference to the UI Panel

    private Animator animator;
    private Rigidbody rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            // Freeze rotation and lock the Y position
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // Prevent tunneling
            rb.interpolation = RigidbodyInterpolation.Interpolate; // Smooth movement
        }

        // Ensure the UI Panel starts hidden
        if (DIalogPanel != null)
        {
            DIalogPanel.SetActive(false);
        }
    }

    void Update()
    {
        // Input handling for WASD or arrow keys
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        movementDirection.Normalize();

        // Update animation based on movement
        if (animator != null)
        {
            bool isWalking = movementDirection != Vector3.zero;
            animator.SetBool("walking", isWalking); // Set the "walking" parameter in the Animator
        }

        // Move the character
        transform.Translate(movementDirection * speed * Time.deltaTime, Space.World);

        // Handle rotation
        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        // Show the UI Panel when the Space bar is pressed
        if (Input.GetKeyDown(KeyCode.Space) && DIalogPanel != null)
        {
            DIalogPanel.SetActive(true); // Show the UI Panel
        }
    }
}
