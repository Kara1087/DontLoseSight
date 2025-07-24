using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;
using Unity.VisualScripting;
using Sequence = DG.Tweening.Sequence;

public class TargetController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float directionChangeInterval = 3f;
    [SerializeField] private InputHandler inputHandler;
    [SerializeField, Range(0f, 1f)] private float verticalInfluence = 0.3f; // 0.3 = amplitude verticale réduite
    [SerializeField] private float rotationSpeed = 360f; // vitesse de roulage au sol
    [SerializeField] private float groundCheckDistance = 0.6f;
    [SerializeField] private float sphereRadius = 0.5f; // rayon du mesh pour un roulage réaliste
    [SerializeField] private LayerMask groundMask;
    
    [SerializeField] private Transform model; // ton mesh, pour le faire tourner visuellement
    [SerializeField] private GroundChecker groundChecker;
    
    private Rigidbody rb;
    private Vector3 moveDirection;
    private float timer;
    private bool isActive;
    public bool canMove = true;
    private bool isGrounded;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }
    void Start()
    {
        if (inputHandler != null)
        {
            inputHandler.OnFirstMove += ActivateMovement;
        }
        
        ChooseNewDirection();
    }
    
    private void OnDestroy()
    {
        if (inputHandler != null)
        {
            inputHandler.OnFirstMove -= ActivateMovement;
        }
    }
    
    private void ActivateMovement()
    {
        isActive = true;
    }

    void Update()
    {
        if (!isActive || !canMove) return;

        // TEST direct raycast pour vérifier
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundMask);

        timer += Time.deltaTime;
        if (timer >= directionChangeInterval)
        {
            timer = 0f;
            ChooseNewDirection();
        }

        // 💡 Seulement au sol, on fait rouler
        if (isGrounded && model != null)
        {
            float distance = moveSpeed * Time.deltaTime;
            float angle = (distance / (2 * Mathf.PI * sphereRadius)) * 360f;
            model.Rotate(Vector3.right, angle, Space.Self);
        }
        else
        {
            // Effet visuel
        }
    }

    private void FixedUpdate()
    {
        if (!isActive || !canMove) return;

        if (isGrounded)
        {
            // ➡️ au sol : mouvement horizontal uniquement
            Vector3 horizontal = new Vector3(moveDirection.x, 0f, moveDirection.z).normalized;
            rb.linearVelocity = horizontal * moveSpeed;
        }
        else
        {
            // ➡️ dans les airs : mouvement libre dans toutes les directions
            rb.linearVelocity = moveDirection * moveSpeed;
        }
    }

    void ChooseNewDirection()
    {
        // Choisit une direction aléatoire dans l’espace
        moveDirection = Random.onUnitSphere;
        
        // Limite la montée/descente
        moveDirection.y *= verticalInfluence; // Gérer amplitude verticale
        moveDirection.Normalize(); // On renormalise pour garder une vitesse constante
    }
    
    public void PlayGrabAnimationAndDestroy()
    {
        // On empêche le mouvement
        canMove = false;

        // On garde l’échelle initiale
        Vector3 originalScale = transform.localScale;

        // On crée une séquence DOTween
        Sequence seq = DOTween.Sequence();

        // Étape 1 : Grossir légèrement (durée 0.2s)
        seq.Append(transform.DOScale(originalScale * 1.2f, 0.2f).SetEase(Ease.OutBack));

        // Étape 2 : Rapetisser à zéro (durée 0.3s)
        seq.Append(transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack));

        // Étape 3 : Quand la séquence est terminée → on détruit l’objet
        seq.OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}
