using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(InputHandler))]
public class PlayerController : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private InputHandler input;
    [SerializeField] private bool isFlyingMode = true;

    [Header("Movement")]
    [SerializeField] private float moveForce = 10f;
    [SerializeField] private float maxSpeed = 8f;
    
    [Header("Jump (if grounded)")]
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private LayerMask groundLayer;
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

    private void Start()
    {
        // Flying mode = no gravity
        rb.useGravity = !isFlyingMode;
    }

    private void Update()
    {
        if (!isFlyingMode)
        {
            isGrounded = CheckGrounded();
        }
    }

    private void FixedUpdate()
    {
        if (isFlyingMode)
        {
            Fly();
        }
        else
        {
            MoveOnGround();
            Jump();
        }
    }

    private void Fly()
    {
        // Déplacement horizontal via MoveInput
        Vector3 moveInput = new Vector3(input.MoveInput.x, 0f, input.MoveInput.y);

        // Ajoute la composante verticale
        Vector3 moveDir = new Vector3(moveInput.x, input.VerticalInput, moveInput.z);

        // Applique la force si sous vitesse max
        if (rb.linearVelocity.magnitude < maxSpeed)
        {
            rb.AddForce(moveDir.normalized * moveForce, ForceMode.Force);
        }
    }

    private void MoveOnGround()
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
            if (GameManager.Instance != null)
            {
                GameManager.Instance.Victory();
            }
        }
    }
}