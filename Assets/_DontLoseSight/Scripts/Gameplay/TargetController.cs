using UnityEngine;
using Random = UnityEngine.Random;

public class TargetController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float directionChangeInterval = 3f;
    [SerializeField] private InputHandler inputHandler;
    [SerializeField, Range(0f, 1f)] private float verticalInfluence = 0.3f; // 0.3 = amplitude verticale réduite
    
    private Vector3 moveDirection;
    private float timer;
    private bool isActive;
    public bool canMove = true;
    
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
        if (!isActive) return;
        if (!canMove) return;
        
        timer += Time.deltaTime;
        if (timer >= directionChangeInterval)
        {
            timer = 0f;
            ChooseNewDirection();
        }
        // Mouvement en monde global
        transform.Translate(Time.deltaTime * moveSpeed * moveDirection, Space.World);
    }

    void ChooseNewDirection()
    {
        // Choisit une direction aléatoire dans l’espace
        moveDirection = Random.onUnitSphere;
        
        // Limite la montée/descente
        moveDirection.y *= verticalInfluence; // Gérer amplitude verticale
        moveDirection.Normalize(); // On renormalise pour garder une vitesse constante
    }
}
