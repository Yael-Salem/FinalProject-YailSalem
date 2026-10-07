using UnityEngine;

public class KeycardDoor : Door, IKeycardUnlockable
{
    public bool HasCard { get; set; } = false;

    protected override void Interact()
    {
        if (!HasCard)
        {
            this.promptMessage = "Keycard Needed";
            return;
        }
        
        base.Interact();
    }
}
