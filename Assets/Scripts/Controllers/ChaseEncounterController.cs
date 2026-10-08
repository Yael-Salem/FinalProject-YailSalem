using System;
using UnityEngine;

public class ChaseEncounterController : MonoBehaviour
{
    [SerializeField] private GameObject chaseEnemyPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private CutsceneTrigger trigger;
    [SerializeField] private string cutsceneId;

    private ChaseEnemy spawnedEnemy;
    private bool hasStarted = false;
    
    // Function used to start the sequence (cutscene and then making the enemy chase the player)
    public void StartEncounter()
    {
        if (hasStarted || chaseEnemyPrefab == null || spawnPoint == null || trigger == null)
            return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null || !player.TryGetComponent<InputManager>(out var inputManager))
            return;

        hasStarted = true;
        
        // Spawning the enemy first so it is visible during the cutscene
        GameObject enemy = Instantiate(chaseEnemyPrefab, spawnPoint.position, spawnPoint.rotation);
        spawnedEnemy = enemy.GetComponent<ChaseEnemy>();
        
        // Starting the cutscene
        trigger.activeInputManager = inputManager;
        trigger.onCutsceneEnded += HandleCutsceneEnded;
        trigger.StartCutscene(player, cutsceneId);
    }

    private void HandleCutsceneEnded()
    {
        trigger.onCutsceneEnded -= HandleCutsceneEnded;
        
        if(spawnedEnemy != null)
            spawnedEnemy.StartChase();
    }

    private void OnDestroy()
    {
        if (trigger != null)
            trigger.onCutsceneEnded -= HandleCutsceneEnded;
    }
}
