using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string promptMessage;


    // This function will be called from the player script
    public void BaseInteract()
    {
        Interact();
    }

    protected abstract void Interact();
}
