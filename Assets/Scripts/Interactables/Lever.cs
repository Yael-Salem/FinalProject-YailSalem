using System;
using System.Collections;
using UnityEngine;

public class Lever : Interactable
{
    // Reference to the gate the lever will open
    [SerializeField] private Gate gate;
    
    [Header("Lever animation")]
    [SerializeField] private Transform leverHandle;


    [SerializeField] private float pulledZOffset = 77.248f;
    [SerializeField] private float pullSpeed = 120f;
    
    // Static counter to keep track of how many out of the 3 levers the player has pulled
    private static int leverPulledCount = 0;
    
    // Getter for the counter
    public static int LeverPulledCount
    {
        get => leverPulledCount;
    }
    
    // Cutscene trigger to start the scene for the third lever pull
    [SerializeField] private CutsceneTrigger trigger;

    // Boolean value to prevent the same lever being pulled multiple times
    private bool hasBeenPulled = false;

    private Coroutine pullCoroutine;

    private void Awake()
    {
        if (leverHandle == null)
            leverHandle = transform;
    }

    protected override void Interact()
    {
        if (hasBeenPulled)
            return;

        hasBeenPulled = true;
        
        
        if(gate != null)
            gate.Open();

        if (leverHandle != null)
        {
            if (pullCoroutine != null)
                StopCoroutine(pullCoroutine);

            pullCoroutine = StartCoroutine(PullHandle());
        }

        if (leverPulledCount < 3)
        {
            leverPulledCount++;
            
            // Checking if the final lever has been pulled and triggering a different objective if it has
            if (leverPulledCount == 3)
            {
                // GameObject player = GameObject.FindGameObjectWithTag("Player");
                // DialogueManager.Instance.StartDialogue("observation_room_scene_start", player, () =>
                // {
                //     ObjectiveManager.Instance.TriggerObjective("survive");
                // });
                
                GameObject player = GameObject.FindWithTag("Player");

                if (player != null && player.TryGetComponent<InputManager>(out var inputManager))
                {
                    trigger.activeInputManager = inputManager;
            
                    trigger.StartCutscene(player, "observation_room_scene_start");
                    
                    ObjectiveManager.Instance.TriggerObjective("survive");
                }
            }
                

            else
                ObjectiveManager.Instance.TriggerObjective($"override_{leverPulledCount}");
            
        }
    }

    private IEnumerator PullHandle()
    {
        Vector3 startEuler = leverHandle.localEulerAngles;
        float startZ = startEuler.z;
        float targetZ = startZ + pulledZOffset;
 
        float t = 0f;
        float duration = Mathf.Abs(pulledZOffset) / pullSpeed;
 
        while (t < duration)
        {
            t += Time.deltaTime;
            float currentZ = Mathf.LerpAngle(startZ, targetZ, t / duration);
            leverHandle.localEulerAngles = new Vector3(startEuler.x, startEuler.y, currentZ);
            yield return null;
        }
 
        leverHandle.localEulerAngles = new Vector3(startEuler.x, startEuler.y, targetZ);


    }
}
