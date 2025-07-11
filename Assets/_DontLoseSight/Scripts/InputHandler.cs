using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    private bool jumpPressed = false;

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
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpPressed = true;
        }
    }
}