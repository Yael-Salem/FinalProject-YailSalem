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

    private InputManager activeInputManager;
    private bool isCutsceneActive = false;

    private void Start()
    {
        // Checking if the cutscene ID is found in the list of watched cutscenes and disabling the collider if it is
        if (SaveManager.Instance != null &&
            SaveManager.Instance.playerSaveData.watchedCutscenesID.Contains(cutsceneSaveId))
            GetComponent<Collider>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Not playing the cutscene if the player has already seen it
        if (SaveManager.Instance != null &&
            SaveManager.Instance.playerSaveData.watchedCutscenesID.Contains(cutsceneSaveId))
            return;
        
        
        if (other.CompareTag("Player") && other.TryGetComponent<InputManager>(out var inputManager))
        {
            activeInputManager = inputManager;
            StartCutscene(other.gameObject);
            GetComponent<Collider>().enabled = false;
        }
    }

    private void StartCutscene(GameObject player)
    {
        isCutsceneActive = true;
        activeInputManager.SetCutsceneMode(true);
        activeInputManager.uiActions.Submit.performed += OnSubmitPressed;
        
        DialogueManager.Instance.StartDialogue(cutsceneId, player, EndCutscene);
    }

    private void EndCutscene()
    {
        isCutsceneActive = false;
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
