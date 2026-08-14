using UnityEngine;

public class CodeDoor : Door
{
    [SerializeField] private string correctCode = "7294";

    public bool TrySubmitCode(string enteredCode)
    {
        if (enteredCode == correctCode)
        {
            this.isLocked = false;

            this.promptMessage = "Unlocked";
            
            return true;
        }

        return false;
    }
}
