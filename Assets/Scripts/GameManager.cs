using UnityEngine;
using System;
using System.Collections.Generic;

public enum GamePhase
{
    Intro, // 1
    LobbyScene, // 2
    FirstEncounter, // 3
    SecondLeverPuzzle, // 4
    LabSurvivalSequenceStart, // 5
    LabSurvivalSequenceEnd, // 6
    HallwaySurvival, // 7
    Descent, // 8
    Chase, // 9
    Ending // 10
}

[Serializable]
public class PhaseTransitionRule
{
    public GamePhase targetPhase;
    public List<string> requiredObjectiveIds = new List<string>();
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    // An event that fires whenever a phase changes
    public static event Action<GamePhase, GamePhase> onPhaseChanged;
    
    // An event that fires whenever a generic flag is set or cleared
    public static event Action<string, bool> onFlagChanged;
    
    [Header("Phase Transition Rules")]
    [SerializeField] private List<PhaseTransitionRule> phaseTransitionRules = new List<PhaseTransitionRule>();

    public GamePhase currentPhase { get; private set; } = GamePhase.Intro;
    
    // Generic flags for anything that doesn't warrant a full game phase
    private HashSet<string> activeFlags = new HashSet<string>();
    
    [Header("Debug")]
    [SerializeField] private GamePhase currentPhaseDebugView;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
            return;
        }

        ObjectiveManager.onObjectiveCompleted += HandleObjectiveCompleted;
    }

    private void OnDestroy()
    {
        ObjectiveManager.onObjectiveCompleted -= HandleObjectiveCompleted;
    }

    private void HandleObjectiveCompleted(string completedId)
    {
        Debug.Log($"[GameManager] Objective completed: {completedId}, currentPhase: {currentPhase}");
        
        PhaseTransitionRule bestRule = null;
        
        // Checking every rule since a completed objective might satisfy more than one rule
        foreach (PhaseTransitionRule rule in phaseTransitionRules)
        {
            Debug.Log($"[GameManager] Checking rule -> target: {rule.targetPhase}, required: {string.Join(",", rule.requiredObjectiveIds)}, allComplete: {AllObjectivesComplete(rule.requiredObjectiveIds)}");
            
            if (rule.targetPhase <= currentPhase)
                continue; // The target phase comes before the current phase so we skip it as to not move backwards in the game
            
            if(bestRule != null && rule.targetPhase >= bestRule.targetPhase)
                continue; // Skip, since we already found a better candidate

            if (AllObjectivesComplete(rule.requiredObjectiveIds))
                bestRule = rule;
        }
        
        if (bestRule != null)
            SetPhase(bestRule.targetPhase);
        
        else
            Debug.Log("[GameManager] No qualifying rule found this pass.");
    }
    
    private bool AllObjectivesComplete(List<string> ids)
    {
        foreach (string id in ids)
        {
            if (!ObjectiveManager.Instance.IsObjectiveCompleted(id))
                return false;
        }

        return true;
    }
    
    public void SetPhase(GamePhase newPhase)
    {
        if (newPhase == currentPhase)
            return;

        GamePhase previousPhase = currentPhase;
        currentPhase = newPhase;
        
        // Updating debug view of the current gamephase
        currentPhaseDebugView = newPhase;
        
        Debug.Log($"Game phase changed from: {previousPhase} to {newPhase}");
        onPhaseChanged?.Invoke(previousPhase, newPhase);
        
    }

    // Function to check if we are at a specific phase or later in the game
    public bool IsPhaseAtLeast(GamePhase phase)
    {
        return currentPhase >= phase;
    }

    public void SetFlag(string flagName, bool value)
    {
        if (string.IsNullOrEmpty(flagName))
            return;

        bool changed = value ? activeFlags.Add(flagName) : activeFlags.Remove(flagName);
        
        if(changed)
            onFlagChanged?.Invoke(flagName, value);
    }

    public bool HasFlag(string flagName)
    {
        return activeFlags.Contains(flagName);
    }
}

