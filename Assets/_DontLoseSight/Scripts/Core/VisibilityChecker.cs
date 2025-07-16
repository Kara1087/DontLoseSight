using UnityEngine;

public class VisibilityChecker : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;
    [SerializeField] private float timeBeforeLose = 1f;
    
    private float outOfViewTimer = 0f;
    private Camera mainCam;
    
    void Start()
    {
        mainCam = Camera.main;
    }

    
    void Update()
    {
        if (!target || !mainCam) return;
        
        Vector3 viewportPos = mainCam.WorldToViewportPoint(target.position);
        
        bool isVisible = viewportPos.z > 0 &&
                         viewportPos.x > 0 && viewportPos.x < 1 &&
                         viewportPos.y > 0 && viewportPos.y < 1;
        
        if (isVisible)
        {
            outOfViewTimer = 0f;
        }
        else
        {
            outOfViewTimer += Time.deltaTime;

            if (outOfViewTimer >= timeBeforeLose)
            {
                GameManager.Instance.GameOver();
            }
        }
    }
}
