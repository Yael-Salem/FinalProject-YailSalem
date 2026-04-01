using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string promptMessage;


    // This function will be called from the player script
    public void BaseInteract()
    {
        Interact();
    }
    
    protected virtual void Interact()
    {
        // Template function to be overriden by subclasses
        // Declared as virtual as to not require subclasses to override
    }
}
