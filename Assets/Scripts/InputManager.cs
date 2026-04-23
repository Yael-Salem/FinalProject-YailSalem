using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;

    public PlayerInput.OnFootActions onFoot;

    private PlayerMotor motor;

    private PlayerLook look;

    private PlayerCombat combat;

    private PlayerHealth health;
    
    // Debugging
    private PlayerInput.DebugActions debug;
    
    void Awake()
    {
        playerInput = new PlayerInput();

        onFoot = playerInput.OnFoot;

        debug = playerInput.Debug;

        motor = GetComponent<PlayerMotor>();

        combat = GetComponent<PlayerCombat>();

        onFoot.Jump.performed += ctx => motor.Jump();

        look = GetComponent<PlayerLook>();

        onFoot.Crouch.performed += ctx => motor.Crouch();
        
        // Press and hold for sprinting
        onFoot.Sprint.performed += ctx => motor.Sprint();
        onFoot.Sprint.canceled += ctx => motor.SprintCancel();

        onFoot.Attack.performed += ctx => combat.Attack();
        
        onFoot.Block.performed += ctx => combat.Block();
        onFoot.Block.canceled += ctx => combat.CancelBlock();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        #region DebugControls
        
        // Test damage and heal UI
        health = GetComponent<PlayerHealth>();
        
        debug.Damage.performed += ctx => health.TakeDamage(Random.Range(5, 10));
        debug.Heal.performed += ctx => health.RestoreHealth(Random.Range(5, 10));

        #endregion

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
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }
}
