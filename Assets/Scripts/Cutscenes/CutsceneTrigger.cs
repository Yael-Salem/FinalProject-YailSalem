using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CutsceneTrigger : MonoBehaviour
{
    public string cutsceneId;

    private InputManager activeInputManager;
    private bool isCutsceneActive = false;

    private void OnTriggerEnter(Collider other)
    {
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
    }

    private void OnSubmitPressed(InputAction.CallbackContext obj)
    {
        if(isCutsceneActive)
            DialogueManager.Instance.DisplayNextSentence();
    }
}
