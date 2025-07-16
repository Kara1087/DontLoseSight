using System;
using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    private Animator animator;
    public PlayerController playerController;
    public TargetController targetController;
    
    private bool isGrabbing = false;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
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
        Destroy(targetController.gameObject);
        
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
