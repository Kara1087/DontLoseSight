using System;
using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerModeManager modeManager;
    
    public PlayerController playerController;
    public TargetController targetController;
    
    private Animator animator;
    private bool isGrabbing = false;

    private void Awake()
    {
        if (modeManager == null)
        {
            modeManager = GetComponentInChildren<PlayerModeManager>();
            if (modeManager == null)
            {
                Debug.LogError("❌ Aucun PlayerModeManager trouvé dans PlayerGrab !");
            }
        }
    }
    
    private void Start()
    {
        animator = modeManager.GetActiveAnimator();
        // Mets à jour l'Animator quand on change de mode
        modeManager.OnModeChanged += OnPlayerModeChanged;
    }

    private void OnDestroy()
    {
        // 👉 Désabonnement pour éviter des erreurs à la destruction
        modeManager.OnModeChanged -= OnPlayerModeChanged;
    }

    private void OnPlayerModeChanged(PlayerMode newMode)
    {
        animator = modeManager.GetActiveAnimator();
        Debug.Log("🔄 Animator mis à jour pour le mode : " + newMode);
    }
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (isGrabbing) return;
        if (other.CompareTag("Target"))
        {
            StartGrab();
        }
    }

    private void StartGrab()
    {
        isGrabbing = true;
        playerController.canMove = false;
        if (targetController != null) targetController.canMove = false;
        
        Debug.Log("🎬 Animation Grab lancée à " + Time.time + "s");
        animator.SetTrigger("Grab");
    }

    public void OnGrabAnimationEnd()
    {
        Debug.Log("✅ Animation Grab terminée à " + Time.time + "s");
        
        if (targetController != null)
        {
            targetController.PlayGrabAnimationAndDestroy();
        }
        else
        {
            Debug.LogWarning("⚠️ Pas de TargetController trouvé pour l'animation !");
        }
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.Victory();
        }
        else
        {
            Debug.Log("No GameManager found");
        }
    }
}
