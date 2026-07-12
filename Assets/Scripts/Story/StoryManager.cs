using System;
using UnityEngine;
using System.Collections.Generic;

public class StoryManager : MonoBehaviour
{
    public static StoryManager Instance { get; private set; }

    public static event Action OnStoryStateUpdated;
    
    // A HashSet of all story milestones the player has completed
    private HashSet<string> compeletedMilestones = new HashSet<string>();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        else
            Destroy(gameObject);
    }

    public void CompleteMileStones(string milestoneId)
    {
        if (!compeletedMilestones.Contains(milestoneId))
        {
            compeletedMilestones.Add(milestoneId);
            Debug.Log($"Completed story milestone: {milestoneId}");
            
            OnStoryStateUpdated?.Invoke();
        }
    }

    public bool IsMilestoneComplete(string milestoneId)
    {
        return compeletedMilestones.Contains(milestoneId);
    }
}
