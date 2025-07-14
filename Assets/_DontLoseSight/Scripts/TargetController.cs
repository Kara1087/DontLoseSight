using UnityEngine;

public class TargetController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float directionChangeInterval = 2f;
    
    private Vector3 moveDirection;
    private float timer;
    
    void Start()
    {
        ChooseNewDirection();
    }

    void Update()
    {
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
        float angle = Random.Range(0f, 360f);
        moveDirection = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)).normalized;
    }
}
