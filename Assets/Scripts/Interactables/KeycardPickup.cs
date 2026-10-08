using UnityEngine;

public class KeycardPickup : Interactable
{

    [SerializeField] private string objectiveId; // The objective id that triggers when the keycard is picked up

    [SerializeField] private MonoBehaviour targetObject; // The object that will be unlocked by the keycard
    
    
    [SerializeField] private ChaseEncounterController chaseEncounter; // Optional variable for the chase encounter
    
    protected override void Interact()
    {
        // Checking if the targetObject given can be unlocked by a keycard
        if (targetObject is IKeycardUnlockable unlockable)
            unlockable.HasCard = true;
        
        if(chaseEncounter != null)
            chaseEncounter.StartEncounter();
        
        if(!string.IsNullOrEmpty(objectiveId))
            ObjectiveManager.Instance.TriggerObjective(objectiveId);
        
        Destroy(gameObject);
    }
}
