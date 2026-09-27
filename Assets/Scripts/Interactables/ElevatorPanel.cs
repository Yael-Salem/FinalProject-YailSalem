using UnityEngine;
using UnityEngine.SceneManagement;

public class ElevatorPanel : Interactable
{
    [SerializeField] private string noCardObjectiveId; // The objective that triggers when the player interacts without the keycard (for the first time)

    private bool hasTriggeredObjective = false;
    
    // TODO Change scene name from demo end screen to next level scene
    private string nextSceneName = "DemoEndScreen";
    
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
        
        // TODO Demo end screen, Replace with actual next level
        SceneManager.LoadSceneAsync(nextSceneName);
    }
}
