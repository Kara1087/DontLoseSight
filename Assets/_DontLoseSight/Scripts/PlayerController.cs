using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(InputHandler))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputHandler input;
    [SerializeField] private LayerMask groundLayer;

    [Header("Movement")]
    [SerializeField] private float moveForce = 10f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float maxSpeed = 8f;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private float groundCheckOffset = 0.52f;

    private Rigidbody rb;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (input == null)
            Debug.LogWarning("🎮 InputHandler non assigné !");
    }

    private void Update()
    {
        isGrounded = CheckGrounded();
    }

    private void FixedUpdate()
    {
        Move();
        Jump();
    }

    private void Move()
    {
        Vector3 moveInput = new Vector3(input.MoveInput.x, 0f, input.MoveInput.y);
        Vector3 force = moveInput.normalized * moveForce;

        if (rb.linearVelocity.magnitude < maxSpeed)
        {
            rb.AddForce(force, ForceMode.Force);
        }
    }

    private void Jump()
    {
        if (input.ConsumeJumpPressed() && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private bool CheckGrounded()
    {
        Vector3 origin = transform.position + Vector3.down * groundCheckOffset;
        return Physics.CheckSphere(origin, groundCheckRadius, groundLayer);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            FindObjectOfType<GameManager>().Victory();
        }
    }
}