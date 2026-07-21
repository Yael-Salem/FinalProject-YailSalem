using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CutsceneTrigger : MonoBehaviour
{
    // ID to know which scene to read from the script.json file
    public string cutsceneId;
    
    // ID for saving if we have already watched the cutscene or not
    [ContextMenuItem("Generate new save ID", "GenerateSaveID")]
    [SerializeField] private string cutsceneSaveId;
    
    [SerializeField] private string requiredCutsceneSaveId; // Contains the GUID of another cutscene that is required to view the current one, is empty if no cutscene is required

    [SerializeField] private string requiredObjectiveId; // Contains the ID of an objective that needs to be active in order to view the cutscene, is empty if no objective in required

    public InputManager activeInputManager;
    private bool isCutsceneActive = false;

    private void Start()
    {
        // Checking if the cutscene ID is found in the list of watched cutscenes and disabling the collider if it is
        if (SaveManager.Instance != null &&
            SaveManager.Instance.playerSaveData.watchedCutscenesID.Contains(cutsceneSaveId))
        {
            GetComponent<Collider>().enabled = false;
            return;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // Not playing the cutscene if the player has already seen it
        if (SaveManager.Instance != null &&
            SaveManager.Instance.playerSaveData.watchedCutscenesID.Contains(cutsceneSaveId))
            return;
        
        // Checking if the scene required a previous cutscene to be viewed, and not playing the current scene if the required previous scene hasn't been viewed
        if (!string.IsNullOrEmpty(requiredCutsceneSaveId) &&
            !SaveManager.Instance.playerSaveData.watchedCutscenesID.Contains(requiredCutsceneSaveId))
            return;
        
        // Checking if a certain objective needs to be active before playing, and not playing the scene unless it is active
        if (!string.IsNullOrEmpty(requiredObjectiveId)
            && SaveManager.Instance != null
            && !SaveManager.Instance.playerSaveData.completedObjectivesID.Contains(requiredObjectiveId))
            return;
        
        
        if (other.CompareTag("Player") && other.TryGetComponent<InputManager>(out var inputManager))
        {
            activeInputManager = inputManager;
            StartCutscene(other.gameObject);
            GetComponent<Collider>().enabled = false;
        }
    }
    
    // Function to start a specific cutscene with an optional parameter of a cutscene id to use in the cutscene interact script
    public void StartCutscene(GameObject player, string externalCutsceneId = null)
    {
        isCutsceneActive = true;

        PauseMenuManager.canPause = false;
        
        activeInputManager.SetCutsceneMode(true);
        activeInputManager.uiActions.Submit.performed += OnSubmitPressed;

        string idToPlay = !string.IsNullOrEmpty(externalCutsceneId) ? externalCutsceneId : cutsceneId;
        
        DialogueManager.Instance.StartDialogue(idToPlay, player, EndCutscene);
    }
    
    private void EndCutscene()
    {
        isCutsceneActive = false;

        PauseMenuManager.canPause = true;
        
        activeInputManager.uiActions.Submit.performed -= OnSubmitPressed;
        activeInputManager.SetCutsceneMode(false);

        if (SaveManager.Instance != null && !string.IsNullOrEmpty(cutsceneSaveId))
        {
            SaveManager.Instance.playerSaveData.watchedCutscenesID.Add(cutsceneSaveId);
            Debug.Log($"Cutscene {cutsceneId} has been saved, save ID: {cutsceneSaveId}");
        }
    }

    private void OnSubmitPressed(InputAction.CallbackContext obj)
    {
        if(isCutsceneActive)
            DialogueManager.Instance.DisplayNextSentence();
    }
    
    // Generating a unique save ID for each cutscene using GUID
    private void GenerateSaveID()
    {
        cutsceneSaveId = System.Guid.NewGuid().ToString();
    }
}
