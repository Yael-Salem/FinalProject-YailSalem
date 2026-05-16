using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;

    public PlayerInput.OnFootActions onFoot;

    public PlayerInput.UIActions uiActions;

    private PlayerMotor motor;

    private PlayerLook look;

    private PlayerCombat combat;

    private PlayerHealth health;
    
    // TODO DEBUG DebugActions Reference delete later
    private PlayerInput.DebugActions debug;
    
    void Awake()
    {
        playerInput = new PlayerInput();

        onFoot = playerInput.OnFoot;

        uiActions = playerInput.UI;

        // TODO DEBUG Delete this line
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

        onFoot.OpenInventory.performed += ctx => HandleInventoryInput();
        uiActions.CloseInventory.performed += ctx => HandleInventoryInput();
        
        // TODO DEBUG Controls delete later
        #region DebugControls
        
        // Test damage and heal UI
        health = GetComponent<PlayerHealth>();
        
        debug.Damage.performed += ctx => health.TakeDamage(Random.Range(5, 10));
        debug.Heal.performed += ctx => health.RestoreHealth(Random.Range(5, 10));

        debug.Save.performed += ctx =>
        {
            if (SaveManager.Instance != null)
            {
                SaveManager.Instance.SaveGame(transform.position);
                Debug.Log("DEBUG: Game saved");
            }
        };

        debug.Load.performed += ctx =>
        {
            if (SaveManager.Instance != null)
            {
                SaveManager.Instance.LoadGame();
                Debug.Log("DEBUG: Game Loaded");
            }
        };

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
        playerInput.OnFoot.Enable();
        playerInput.UI.Enable();
        
        // TODO DEBUG controls enable delete later
        playerInput.Debug.Enable();
    }

    private void OnDisable()
    {
        playerInput.OnFoot.Disable();
        playerInput.UI.Disable();
        
        // TODO DEBUG controls disable delete later
        playerInput.Debug.Disable();
    }

    // Function to handle disabling player movement when they open the inventory and then re-enabling it once they close the inventory
    public void HandleInventoryInput()
    {
        InventoryManager.Instance.ToggleInventory();

        if (onFoot.enabled)
        {
            onFoot.Disable();
            uiActions.Enable();
        }

        else
        {
            uiActions.Disable();
            onFoot.Enable();
        }
    }

    public void SetCutsceneMode(bool inCutscene)
    {
        if (inCutscene)
        {
            // Stopping all player movement if they are in a cutscene
            onFoot.Disable();
            uiActions.Enable();
            
            motor.ProcessMove(Vector2.zero);
            look.ProcessLook(Vector2.zero);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

        }

        else
        {
            uiActions.Disable();
            onFoot.Enable();
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

        }
    }
}
