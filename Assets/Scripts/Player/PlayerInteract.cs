using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Camera cam;

    // Max distance for the ray to shoot outwards
    [SerializeField]
    private float distance = 3f;

    [SerializeField]
    private LayerMask mask;

    private PlayerUI playerUI;

    private InputManager inputManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = GetComponent<PlayerLook>().cam;

        playerUI = GetComponent<PlayerUI>();

        inputManager = GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        playerUI.UpdateText(string.Empty);
        
        // Creating a ray that starts at the center of the camera and shoots outwards
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        RaycastHit hitInfo; // Variable that stores collision information

        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            if (hitInfo.collider.GetComponent<Interactable>() != null)
            {
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                
                playerUI.UpdateText(interactable.promptMessage);

                if (inputManager.onFoot.Interact.triggered)
                {
                    interactable.BaseInteract();
                }
                
            }
        }
    }
}
