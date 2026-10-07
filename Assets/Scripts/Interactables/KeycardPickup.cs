using UnityEngine;

public class KeycardPickup : Interactable
{

    [SerializeField] private string objectiveId; // The objective id that triggers when the keycard is picked up

    [SerializeField] private MonoBehaviour targetObject; // The object that will be unlocked by the keycard
    
    protected override void Interact()
    {
        // Checking if the targetObject given can be unlocked by a keycard
        if (targetObject is IKeycardUnlockable unlockable)
            unlockable.HasCard = true;
        
        if(!string.IsNullOrEmpty(objectiveId))
            ObjectiveManager.Instance.TriggerObjective(objectiveId);
        
        Destroy(gameObject);
    }
}
