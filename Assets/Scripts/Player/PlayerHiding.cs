using UnityEngine;

public class PlayerHiding : MonoBehaviour
{
    public bool isHiding { get; private set; }

    private Vector3 positionBeforeHiding;
    private InputManager inputManager;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputManager = GetComponent<InputManager>();

        isHiding = false;
    }

    public void Hide(Vector3 hidingSpotPosition)
    {
        Debug.Log("Hide start");
    
        isHiding = true;
        positionBeforeHiding = transform.position;

        if (TryGetComponent<CharacterController>(out var controller))
            controller.enabled = false;

        transform.position = hidingSpotPosition;

        if (controller != null)
            controller.enabled = true;
    
        Debug.Log("Hide end");
    }

    public void ExitHidingSpot()
    {
        Debug.Log("ExitHidingSpot start");
    
        isHiding = false;

        if (TryGetComponent<CharacterController>(out var controller))
            controller.enabled = false;

        transform.position = positionBeforeHiding;

        if (controller != null)
            controller.enabled = true;
    
        Debug.Log("ExitHidingSpot start");
    }
    
}
