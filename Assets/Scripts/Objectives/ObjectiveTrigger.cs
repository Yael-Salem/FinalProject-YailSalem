using System;
using UnityEngine;

public class ObjectiveTrigger : MonoBehaviour
{
    [SerializeField] private string objectiveId; // The ID of the objective that the trigger will start

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            ObjectiveManager.Instance.TriggerObjective(objectiveId);
    }
}
