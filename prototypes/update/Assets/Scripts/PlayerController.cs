using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float jumpForce = 8f;
    public int maxJumps = 2;

    [Header("Dash")]
    public float dashForce = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header("Camera")]
    public Camera playerCamera; // Use this instead of Camera.main
    public float mouseSensitivity = 100f;

    private Rigidbody rb;
    private Vector3 moveInput;
    private bool isGrounded;
    private int jumpCount;
    private bool isDashing;
    private bool canDash = true;
    private Vector3 dashDirection;
    private float xRotation = 0f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;

        // Default to Camera.main if not set
        if (playerCamera == null && Camera.main != null)
        {
            playerCamera = Camera.main;
        }
    }

    private void Update()
    {
        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && !isDashing)
            jumpCount = 0;

        // Prevent null camera errors
        if (playerCamera == null) return;

        // Movement input
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 camForward = playerCamera.transform.forward;
        Vector3 camRight = playerCamera.transform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        moveInput = (camRight * h + camForward * v).normalized;

        // Mouse movement input for camera rotation
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);

        // Vertical camera rotation (up/down)
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Prevent over-rotation
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumps)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpCount++;
        }

        // Dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && moveInput.magnitude > 0)
        {
            StartCoroutine(Dash());
        }
    }

    private void FixedUpdate()
    {
        if (!isDashing)
        {
            Vector3 move = moveInput * moveSpeed;
            Vector3 velocity = new Vector3(move.x, rb.linearVelocity.y, move.z);
            rb.linearVelocity = velocity;
        }
    }

    private System.Collections.IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;
        dashDirection = moveInput.normalized;

        rb.linearVelocity = dashDirection * dashForce;
        yield return new WaitForSeconds(dashDuration);

        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
