using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FreezingBehaviour : PowerUp
{
    PlayerMovement playerMovement;
    InputActionReference inputRef;
    int counter = 5;

    private void Start()
    {
        playerMovement = affected.GetComponent<PlayerMovement>();
        playerMovement.UnsubscribeToAction();
        inputRef = playerMovement.MoveAction;
        inputRef.action.performed += OnPerformedAction;
    }

    void OnPerformedAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log(counter);
            counter--;
            if(counter <= 0)
            {
                Destroy(this);
            }
        }
    }

    private void OnDestroy()
    {
        inputRef.action.performed -= OnPerformedAction;
        playerMovement.SubscribeToAction();
    }
}
