using UnityEngine;
using  UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool JumpReleased { get; private set; }
    
    private void LateUpdate()
    {
        // Reset every frames
        JumpPressed = false;
        JumpReleased = false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
            JumpPressed = true;

        if (context.canceled)
            JumpReleased = true;
    }
}
