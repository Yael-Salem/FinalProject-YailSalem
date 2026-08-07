using System;
using System.Collections;
using UnityEngine;
using UnityEngine.LowLevelPhysics2D;

public class Door : Interactable
{
    private float openAngle = 90f;
    [SerializeField] private float openSpeed = 250f;
    private bool isOpen;

    private float currentAngle = 0f;
    private Coroutine movementCoroutine;

    private Quaternion initialRotation;

    [SerializeField] private bool isLocked;
    
    
    // An enum to decide if a door is locked for the entire duration of the game or opens at some point
    private enum LockMode
    {
        PhaseGated,
        Permanent
    }

    [SerializeField] private LockMode lockMode = LockMode.PhaseGated; // Each door opens at some point by default
    [SerializeField] private GamePhase requiredPhase;

    private void OnEnable()
    {
        GameManager.onPhaseChanged += HandlePhaseChanged;
        Debug.Log($"{gameObject.name} subscribed to phase changes");
    }

    private void OnDisable()
    {
        GameManager.onPhaseChanged -= HandlePhaseChanged;
    }

    private void Start()
    {
        RefreshLockState();
    }

    private void RefreshLockState()
    {
        if (lockMode == LockMode.Permanent || GameManager.Instance == null)
            return;

        isLocked = !GameManager.Instance.IsPhaseAtLeast(requiredPhase);
    }

    private void HandlePhaseChanged(GamePhase previousPhase, GamePhase newPhase)
    {
        Debug.Log($"{gameObject} received phase change: {newPhase}, isLocked will become {!GameManager.Instance.IsPhaseAtLeast(requiredPhase)}");
        RefreshLockState();
    }

    protected override void Interact()
    {
        if (isLocked)
        {
            this.promptMessage = "Locked";
            return; 
        }
        
        isOpen = !isOpen;
        
        if(movementCoroutine != null)
            StopCoroutine(movementCoroutine);

        movementCoroutine = StartCoroutine(ToggleDoor());
    }

    private IEnumerator ToggleDoor()
    {
        float targetAngle = isOpen ? openAngle : 0f;


        while (Mathf.Abs(currentAngle - targetAngle) > 0.05f)
        {
            float nextAngle = Mathf.MoveTowards(currentAngle, targetAngle, Time.deltaTime * openSpeed);

            float angleDelta = nextAngle - currentAngle;

            float halfWidth = transform.localScale.x / 2f; // Getting half the width of the door so it opens with the right side as it's point of rotation

            Vector3 hingePoint = transform.position + (transform.right * halfWidth);
            
            transform.RotateAround(hingePoint, transform.up, angleDelta);

            currentAngle = nextAngle;
            
            yield return null;
        }
        
        movementCoroutine = null;
    }
}

