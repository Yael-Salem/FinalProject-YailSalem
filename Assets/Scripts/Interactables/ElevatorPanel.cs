using UnityEngine;

public class ElevatorPanel : Interactable
{
    [SerializeField] private string noCardObjectiveId; // The objective that triggers when the player interacts without the keycard (for the first time)

    private bool hasTriggeredObjective = false;
    
    protected override void Interact()
    {
        if (!KeycardPickup.HasCard)
        {
            this.promptMessage = "Keycard needed";

            if (!hasTriggeredObjective && !string.IsNullOrEmpty(noCardObjectiveId))
            {
                hasTriggeredObjective = true;
                ObjectiveManager.Instance.TriggerObjective(noCardObjectiveId);
            }
            
            
            return;
        }

        Debug.Log("Elevator activated, load next level");
    }
}
