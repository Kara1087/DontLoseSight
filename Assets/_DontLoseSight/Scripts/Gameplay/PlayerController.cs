using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(InputHandler))]

public class PlayerController : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private InputHandler input;
    [SerializeField] private PlayerModeManager modeManager;
    

    [Header("Movement")]
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float moveForce = 10f;
    [SerializeField] private float maxSpeed = 8f;
    
    [Header("Jump (Humanoid)")]
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private GroundChecker groundChecker;
    
    private bool isGrounded;
    private Rigidbody rb;
    private Animator animator;
    public bool canMove = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();

        if (input == null)
            Debug.LogWarning("🎮 InputHandler non assigné !");
    }
    
    private void OnEnable()
    {
        // ✅ Abonnement à l’événement du modeManager
        if (modeManager != null)
        {
            modeManager.OnModeChanged += HandleModeChanged;
        }
    }

    private void OnDisable()
    {
        // ✅ Désabonnement pour éviter les erreurs
        if (modeManager != null)
        {
            modeManager.OnModeChanged -= HandleModeChanged;
        }
    }

    private void Start()
    {
        // Cache le curseur
        Cursor.visible = false;

        // Verrouille le curseur au centre de la fenêtre
        Cursor.lockState = CursorLockMode.Locked;
        
        // Initialise l’Animator selon le mode courant
        HandleModeChanged(modeManager.CurrentMode);
    }
    
    private void HandleModeChanged(PlayerMode newMode)
    {
        animator = modeManager.GetActiveAnimator();
        Debug.Log($"🎬 Animator Controller assigné : {animator.runtimeAnimatorController.name}");
    }


    private void Update()
    {
        if (!canMove) return;
        
        // Ground check et mode
        bool wasGrounded = isGrounded;
        isGrounded = groundChecker.IsGrounded;
        
        // Switch auto
        if (isGrounded && modeManager.CurrentMode != PlayerMode.Humanoid)
        {
            modeManager.SetMode(PlayerMode.Humanoid);
        }
        else if (!isGrounded && modeManager.CurrentMode != PlayerMode.Drone)
        {
            modeManager.SetMode(PlayerMode.Drone);
        }
        
        // Switch manuel
        
        // Animator Jammo
        if (modeManager.CurrentMode == PlayerMode.Humanoid && animator != null)
        {
            float inputX = input.MoveInput.x;
            float inputZ = input.MoveInput.y;
            float speed = new Vector2(inputX, inputZ).sqrMagnitude;

            // Blend
            animator.SetFloat("Blend", speed, 0.1f, Time.deltaTime);
        }
        
        // 🔄 Rotation progressive
        if (modeManager.CurrentMode == PlayerMode.Humanoid || modeManager.CurrentMode == PlayerMode.Drone)
        {
            RotateTowardsMovement();
        }
        
    }

    private void FixedUpdate()
    {
        if (!canMove) return;
        
        switch (modeManager.CurrentMode)
        {
            case PlayerMode.Humanoid:
                HandleGroundMovement();
                HandleJump();
                break;

            case PlayerMode.Drone:
                HandleFlight();
                break;
        }
    }

    private void HandleFlight()
    {
        Vector3 inputDir = new Vector3(input.MoveInput.x, 0f, input.MoveInput.y);
        Vector3 desiredDir = new Vector3(inputDir.x, input.VerticalInput, inputDir.z);
        // Déplacement direct
        rb.linearVelocity = desiredDir * maxSpeed;
    }

    private void HandleGroundMovement()
    {
        Vector3 moveInput = new Vector3(input.MoveInput.x, 0f, input.MoveInput.y);
        Vector3 force = moveInput.normalized * moveForce;

        if (rb.linearVelocity.magnitude < maxSpeed)
        {
            rb.AddForce(force, ForceMode.Force);
        }
    }

    private void HandleJump()
    {
        if (input.ConsumeJumpPressed() && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            Debug.Log("🦘 [PlayerController] Jump exécuté !");
        }
    }

    private void RotateTowardsMovement()
    {
        Vector3 inputDir = new Vector3(input.MoveInput.x, 0f, input.MoveInput.y);

        if (inputDir.sqrMagnitude > 0.01f)
        {
            // 👉 On récupère les axes caméra
            Vector3 forward = Camera.main.transform.forward;
            Vector3 right = Camera.main.transform.right;
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            // 👉 On construit la direction de déplacement relative à la caméra
            Vector3 desiredDirection = (forward * inputDir.z + right * inputDir.x).normalized;

            // 👉 On tourne progressivement vers cette direction
            Quaternion targetRotation = Quaternion.LookRotation(desiredDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
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