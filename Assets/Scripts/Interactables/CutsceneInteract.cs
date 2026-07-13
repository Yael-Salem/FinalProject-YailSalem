using System;
using UnityEngine;

public class CutsceneInteract : Interactable
{
    [SerializeField] private string cutsceneId;
    
    protected override void Interact()
    {
        CutsceneTrigger trigger = FindFirstObjectByType<CutsceneTrigger>(FindObjectsInactive.Include);

        GameObject player = GameObject.FindWithTag("Player");

        if (player != null && player.TryGetComponent<InputManager>(out var inputManager))
        {
            trigger.activeInputManager = inputManager;
            
            trigger.StartCutscene(player, cutsceneId);
        }
        
        Destroy(gameObject);
    }
}
