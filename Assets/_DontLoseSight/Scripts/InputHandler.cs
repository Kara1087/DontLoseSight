using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputHandler : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public float VerticalInput { get; private set; }
    private bool jumpPressed = false;
    
    public event Action OnFirstMove;
    private bool hasMovedOnce = false;

    // Appelé une seule fois à la lecture
    public bool ConsumeJumpPressed()
    {
        if (jumpPressed)
        {
            jumpPressed = false;
            return true;
        }
        return false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
        Debug.Log($"[InputHandler] MoveInput = {MoveInput}");
        if (!hasMovedOnce && MoveInput.magnitude > 0.1f)
        {
            hasMovedOnce = true;
            OnFirstMove?.Invoke(); // 🔔 On déclenche l'événement
        }
    }

    public void OnVerticalMove(InputAction.CallbackContext context)
    {
        VerticalInput = context.ReadValue<float>();
        Debug.Log($"[InputHandler] VerticalInput = {VerticalInput}");
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpPressed = true;
            Debug.Log("[InputHandler] Jump pressed!");
        }
    }
}