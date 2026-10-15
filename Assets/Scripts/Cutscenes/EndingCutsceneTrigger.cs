using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndingCutsceneTrigger : CutsceneTrigger
{
    [SerializeField] private GameObject eyes;
    [SerializeField] private float returnToMenuDelay = 2f; // How many seconds to wait before returning to the main menu once the cutscene ends
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private void Awake()
    {
        // Disabling the eyes by default
        if(eyes != null)
            eyes.SetActive(false);
    }

    private void OnEnable()
    {
        onCutsceneEnded += HandleCutsceneEnded;
    }

    private void OnDisable()
    {
        onCutsceneEnded -= HandleCutsceneEnded;
    }

    public override void StartCutscene(GameObject player, string externalCutsceneId = null)
    {
        if(eyes != null)
            eyes.SetActive(true);
        
        base.StartCutscene(player, externalCutsceneId);
    }

    private void HandleCutsceneEnded()
    {
        // Not giving the player back control once the scene ends
        if(activeInputManager != null)
            activeInputManager.onFoot.Disable();

        StartCoroutine(ReturnToMainMenuDelay());
    }

    private IEnumerator ReturnToMainMenuDelay()
    {
        yield return new WaitForSeconds(returnToMenuDelay);

        SceneManager.LoadScene(mainMenuSceneName);
    }
}
