using System;
using UnityEngine;
using System.Collections.Generic;

public class RoomTriggerController : MonoBehaviour
{
    public class TriggerData
    {
        public string cutsceneId;
        public Transform spawnLocation;
        
        [Header("Story Requirements")]
        public string milestoneRequired; // What milestone is required to for this trigger to appear

        public string milestoneToBlockThis; // if this milestone is complete, hide this trigger forever
    }

    public GameObject player;
    public GameObject triggerPrefab;

    public List<TriggerData> potentialTriggers;

    public List<GameObject> activeSpawnedTriggers = new List<GameObject>();

    private void OnEnable()
    {
        StoryManager.OnStoryStateUpdated += RefreshRoomTriggers;
    }

    private void OnDisable()
    {
        StoryManager.OnStoryStateUpdated -= RefreshRoomTriggers;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RefreshRoomTriggers();
    }

    private void RefreshRoomTriggers()
    {
        ClearActiveTriggers();

        foreach (TriggerData data in potentialTriggers)
        {
            bool meetsRequirement = string.IsNullOrEmpty(data.milestoneRequired) ||
                                    StoryManager.Instance.IsMilestoneComplete(data.milestoneRequired);

            bool isBlocked = !string.IsNullOrEmpty(data.milestoneToBlockThis) &&
                             StoryManager.Instance.IsMilestoneComplete(data.milestoneToBlockThis);

            if (meetsRequirement && !isBlocked)
                SpawnTriggerObject(data);
        }
    }

    private void SpawnTriggerObject(TriggerData data)
    {
        GameObject newTrigger =
            Instantiate(triggerPrefab, data.spawnLocation.position, Quaternion.identity, this.transform);
        
        activeSpawnedTriggers.Add(newTrigger);

        if (newTrigger.TryGetComponent<AreaCutsceneTrigger>(out var triggerScript))
            triggerScript.SetupTrigger(data.cutsceneId, player, () =>
            {
                StoryManager.Instance.CompleteMileStones(data.cutsceneId);
            });
    }

    private void ClearActiveTriggers()
    {
        foreach (GameObject obj in activeSpawnedTriggers)
        {
            if(obj != null)
                Destroy(obj);
        }
        
        activeSpawnedTriggers.Clear();
    }
}
