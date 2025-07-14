using UnityEngine;

public class VisibilityChecker : MonoBehaviour
{
    SerializeField] private Transform target;

    void Update()
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(target.position);
        bool isVisible = viewportPos.z > 0 &&
                         viewportPos.x > 0 && viewportPos.x < 1 &&
                         viewportPos.y > 0 && viewportPos.y < 1;

        if (!isVisible)
        {
            Debug.Log("❌ Proie hors champ !");
            // Ici tu pourras appeler GameManager → GameOver()
        }
    }
}
