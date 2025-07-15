using System;
using UnityEngine;

public class AnimationEventGrabRelay : MonoBehaviour
{
    private PlayerGrab playerGrab;

    private void Awake()
    {
        playerGrab = GetComponentInParent<PlayerGrab>();
        if (playerGrab == null)
        {
            Debug.LogWarning("No PlayerGrab found");
        }
    }
    public void Relay_OnGrabAnimationEnd()
    {
        if (playerGrab != null)
        {
            playerGrab.OnGrabAnimationEnd();
        }
    }
}
