using System;
using System.Collections;
using UnityEngine;

public class ElevatorDoor : MonoBehaviour
{
    [SerializeField] private float slideDistance = 2f;
    [SerializeField] private float slideSpeed = 2f;

    private bool isOpen;
    private Coroutine movementCoroutine;
    private Vector3 closedPosition;

    private void Start()
    {
        closedPosition = transform.localPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        SetOpen(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        SetOpen(false);
    }

    private void SetOpen(bool open)
    {
        if (isOpen == open)
            return;

        isOpen = open;
        
        if(movementCoroutine != null)
            StopCoroutine(movementCoroutine);

        movementCoroutine = StartCoroutine(ToggleDoor());
    }

    private IEnumerator ToggleDoor()
    {
        Vector3 targetPosition = isOpen ? closedPosition + new Vector3(slideDistance, 0f, 0f) : closedPosition;

        while (Vector3.Distance(transform.localPosition, targetPosition) > 0.01f)
        {
            transform.localPosition =
                Vector3.MoveTowards(transform.localPosition, targetPosition, Time.deltaTime * slideSpeed);
            yield return null;
        }

        transform.localPosition = targetPosition;
        movementCoroutine = null;
    }
}
