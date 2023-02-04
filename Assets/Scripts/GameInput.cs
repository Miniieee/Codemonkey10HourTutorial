using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteractAction;
    private PlayerInputActions playerInputActions;

    private void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Intaract.performed += Intreract_performed;
    }
    
    private void Intreract_performed(InputAction.CallbackContext obj)
    {
        // ? check if not null, but we need to invoke afterwards
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputvector = new Vector2(0,0);
        inputvector = playerInputActions.Player.Move.ReadValue<Vector2>();

        inputvector = inputvector.normalized;

        return inputvector;
    }
}
