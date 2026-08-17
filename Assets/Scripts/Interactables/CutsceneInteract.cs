using System;
using UnityEngine;

public class CutsceneInteract : Interactable
{
    [SerializeField] private bool destroyAfterUse = true;
    
    [SerializeField] private CutsceneTrigger trigger; // An empty and unused cutscene trigger in order to start and finish the current cutscene without messing up other scenes
    
    private string cutsceneId;
    
    protected override void Interact()
    {
        if (trigger != null)
            cutsceneId = trigger.CutsceneId;

        GameObject player = GameObject.FindWithTag("Player");

        if (player != null && player.TryGetComponent<InputManager>(out var inputManager))
        {
            trigger.activeInputManager = inputManager;
            
            trigger.StartCutscene(player, cutsceneId);
        }
        
        if(destroyAfterUse)
            Destroy(gameObject);
    }
}
