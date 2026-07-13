using UnityEngine;
using System;

public class ObjectiveInteract : Interactable
{
    [SerializeField] private string objectiveId;
    
    protected override void Interact()
    {
        if(ObjectiveManager.Instance != null)
            ObjectiveManager.Instance.TriggerObjective(objectiveId);
        
        else
            Debug.LogWarning($"No Objective manager found in this scene");
        
        Destroy(gameObject);
    }
}
