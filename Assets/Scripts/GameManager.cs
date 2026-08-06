using UnityEngine;
using System;
using System.Collections.Generic;

public enum GamePhase
{
    Intro,
    LobbyScene,
    FirstEncounter,
    SecondLeverPuzzle,
    LabSurvivalSequenceStart,
    LabSurvivalSequenceEnd,
    HallwaySurvival,
    Descent,
    Chase,
    Ending
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
        // Checking every rule since a completed objective might satisfy more than one rule
        foreach (PhaseTransitionRule rule in phaseTransitionRules)
        {
            if (rule.targetPhase <= currentPhase)
                continue; // The target phase comes before the current phase so we skip it as to not move backwards in the game

            if (AllObjectivesComplete(rule.requiredObjectiveIds))
                SetPhase(rule.targetPhase);
        }
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

