using System;
using UnityEngine;

public class HidingSpot : Interactable
{
    public PlayerHiding playerHiding;

    public Transform hideSpot;

    private void Start()
    {
        playerHiding = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHiding>();
    }

    protected override void Interact()
    {
        if (playerHiding != null)
        {

            bool currentlyHiding = playerHiding.isHiding;

            if (!currentlyHiding)
            {
                playerHiding.Hide(hideSpot != null ? hideSpot.position : transform.position);

                if (playerHiding.TryGetComponent<InputManager>(out var input))
                    input.onFoot.Movement.Disable();
            }
            else
            {
                playerHiding.ExitHidingSpot();

                if (playerHiding.TryGetComponent<InputManager>(out var input))
                    input.onFoot.Movement.Enable();
            }
        }
    }
}