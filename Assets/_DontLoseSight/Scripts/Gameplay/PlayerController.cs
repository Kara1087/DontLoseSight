using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(InputHandler))]

public class PlayerController : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private InputHandler input;
    [SerializeField] private bool isFlyingMode = true;
    
    [Header("Visual")]
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private Transform model; // ton mesh enfant
    [SerializeField] private float maxPitch = 30f; // inclinaison max

    [Header("Movement")]
    [SerializeField] private float moveForce = 10f;
    [SerializeField] private float maxSpeed = 8f;
    
    [Header("Jump (if grounded)")]
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private float groundCheckOffset = 0.52f;
    
    private Rigidbody rb;
    private Animator animator;
    private bool isGrounded;
    public bool canMove = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();

        if (input == null)
            Debug.LogWarning("🎮 InputHandler non assigné !");
    }

    private void Start()
    {
        // Flying mode = no gravity
        rb.useGravity = !isFlyingMode;
        
        // Cache le curseur
        Cursor.visible = false;

        // Verrouille le curseur au centre de la fenêtre
        Cursor.lockState = CursorLockMode.Locked;
        
    }

    private void Update()
    {
        if (!canMove) return;
        
        if (!isFlyingMode)
        {
            isGrounded = CheckGrounded();
        }
    }

    private void FixedUpdate()
    {
        if (!canMove) return;
        
        if (isFlyingMode)
        {
            Fly();
        }
        else
        {
            MoveOnGround();
            Jump();
        }
        
        UpdateModelRotation();
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

    private void UpdateModelRotation()
    {
        Vector3 velocity = rb.linearVelocity;
        if (velocity.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(velocity.normalized, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            
        }
    }
    
    private void OnDrawGizmos()
    {
        // On ne dessine les gizmos que si on a un Rigidbody
        if (rb == null) return;

        // Position de départ
        Vector3 start = transform.position;

        // Vitesse horizontale
        Vector3 velocity = rb.linearVelocity;

        // Couleur
        Gizmos.color = Color.cyan;

        // Dessine une flèche (une ligne simple pour commencer)
        Gizmos.DrawLine(start, start + velocity);

        // Dessine une petite sphère au bout
        Gizmos.DrawSphere(start + velocity, 0.1f);
    }
}