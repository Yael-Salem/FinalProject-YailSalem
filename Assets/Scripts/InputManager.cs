using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;

    public PlayerInput.OnFootActions onFoot;

    private PlayerMotor motor;

    private PlayerLook look;

    private PlayerCombat combat;
    
    void Awake()
    {
        playerInput = new PlayerInput();

        onFoot = playerInput.OnFoot;

        motor = GetComponent<PlayerMotor>();

        combat = GetComponent<PlayerCombat>();

        onFoot.Jump.performed += ctx => motor.Jump();

        look = GetComponent<PlayerLook>();

        onFoot.Crouch.performed += ctx => motor.Crouch();
        
        // Press and hold for sprinting
        onFoot.Sprint.performed += ctx => motor.Sprint();
        onFoot.Sprint.canceled += ctx => motor.SprintCancel();

        onFoot.Attack.performed += ctx => combat.Attack();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Tell the PlayerMotor to move using the value from our movement action
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        onFoot.Enable();
    }

    private void OnDisable()
    {
        onFoot.Disable();
    }
}
