using UnityEngine;
using System;

public class PlayerModeManager : MonoBehaviour
{
    [Header("Models")]
    [SerializeField] private GameObject humanoidModel;
    [SerializeField] private GameObject droneModel;

    [Header("References")]
    [SerializeField] private Rigidbody rb;

    public PlayerMode CurrentMode { get; private set; } = PlayerMode.Humanoid;
    
    public event Action<PlayerMode> OnModeChanged;

    private void Awake()
    {
        if (rb == null) rb = GetComponentInParent<Rigidbody>();
    }

    public void SetMode(PlayerMode newMode)
    {
        if (CurrentMode == newMode) return;

        CurrentMode = newMode;
        switch (CurrentMode)
        {
            case PlayerMode.Humanoid:
                rb.useGravity = true;
                humanoidModel.SetActive(true);
                droneModel.SetActive(false);
                Debug.Log("🤖 Mode Humanoid activé");
                break;

            case PlayerMode.Drone:
                rb.useGravity = false;
                humanoidModel.SetActive(false);
                droneModel.SetActive(true);
                Debug.Log("🛸 Mode Drone activé");
                break;
        }
        OnModeChanged?.Invoke(CurrentMode);
    }
    
    public Animator GetActiveAnimator()
    {
        GameObject active = (CurrentMode == PlayerMode.Humanoid) ? humanoidModel : droneModel;
        return active.GetComponent<Animator>();
    }
}