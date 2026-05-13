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
    
    protected override void Interact()
    {
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

