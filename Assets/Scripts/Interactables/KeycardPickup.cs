using UnityEngine;

public class KeycardPickup : Interactable
{

    [SerializeField] private string objectiveId; // The objective id that triggers when the keycard is picked up

    private static bool hasCard = false;
    public static bool HasCard => hasCard;
    
    protected override void Interact()
    {
        hasCard = true;
        
        if(!string.IsNullOrEmpty(objectiveId))
            ObjectiveManager.Instance.TriggerObjective(objectiveId);
        
        Destroy(gameObject);
    }
}
