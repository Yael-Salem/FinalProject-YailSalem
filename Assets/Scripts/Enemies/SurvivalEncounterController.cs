using System;
using UnityEngine;
using System.Collections.Generic;

public class SurvivalEncounterController : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;

    [SerializeField] private GamePhase survivalStartPhase;
    [SerializeField] private GamePhase survivalEndPhase;

    [SerializeField] private CutsceneTrigger trigger; // Trigger used to start cutscene after the sequence ends

    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private int enemiesRemaining;

    private void OnEnable()
    {
        GameManager.onPhaseChanged += HandlePhaseChanged;
        EnemyHealth.onEnemyDied += HandleEnemyDied;
    }

    private void OnDisable()
    {
        GameManager.onPhaseChanged -= HandlePhaseChanged;
        EnemyHealth.onEnemyDied -= HandleEnemyDied;
    }

    private void HandlePhaseChanged(GamePhase previousPhase, GamePhase newPhase)
    {
        if (newPhase != survivalStartPhase)
            return;
        
        spawnedEnemies.Clear();
        enemiesRemaining = spawnPoints.Length;

        foreach (Transform point in spawnPoints)
        {
            GameObject enemy = Instantiate(enemyPrefab, point.position, point.rotation);
            spawnedEnemies.Add(enemy);
        }
    }
    
    private void HandleEnemyDied(GameObject enemy)
    {
        if (!spawnedEnemies.Contains(enemy))
            return;

        spawnedEnemies.Remove(enemy);
        enemiesRemaining--;

        if (enemiesRemaining <= 0)
        {
            if (trigger != null)
            {
                string cutsceneId = trigger.CutsceneId;
                
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                
                trigger.StartCutscene(player, cutsceneId);
            }
                
        }
            
    }
}
