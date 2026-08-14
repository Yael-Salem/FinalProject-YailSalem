using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance { get; private set; }

    [SerializeField] private ObjectiveUI objectiveUI;
    
    // Dictionary containing the objective's id as the key, and it's title (the text displayed to the player) as the value
    private Dictionary<string, string> objectiveDatabase = new Dictionary<string, string>();

    private const string OBJECTIVES_FILE_NAME = "objectives.json";

    public string currentObjectiveTitle { get; set; } = "No active objective";
    
    public string currentObjectiveId { get; set; }
    
    public static event Action<string> onObjectiveCompleted;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadObjectivesFromFile();
        }
        
        else
            Destroy(gameObject);
    }

    private void LoadObjectivesFromFile()
    {
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, OBJECTIVES_FILE_NAME);

        if (File.Exists(filePath))
        {
            string jsonText = File.ReadAllText(filePath);

            ObjectiveDatabase database = JsonUtility.FromJson<ObjectiveDatabase>(jsonText);

            foreach (var item in database.objectives)
            {
                if(!objectiveDatabase.ContainsKey(item.id))
                    objectiveDatabase.Add(item.id, item.title);
                
                else
                    Debug.LogWarning($"Duplicate Objective ID: {item.id}");
            }

            Debug.Log("Objectives Loaded successfully");
        }
        
        else
            Debug.LogError($"Objectives failed to load, file not found at: {filePath}");
    }
    
    public void TriggerObjective(string id)
    {
        if (string.IsNullOrEmpty(id))
            return;

        if (SaveManager.Instance != null && !SaveManager.Instance.playerSaveData.completedObjectivesID.Contains(id))
            SaveManager.Instance.playerSaveData.completedObjectivesID.Add(id);

        if (objectiveDatabase.TryGetValue(id, out string objectiveTitle))
        {
            Debug.Log($"New objective activated: {objectiveTitle}");

            currentObjectiveTitle = objectiveTitle;

            currentObjectiveId = id;

            if (objectiveUI != null)
                objectiveUI.ShowNewObjective(objectiveTitle);
            
            else
                Debug.LogWarning($"No objective UI script assigned");
        }
        
        else
            Debug.LogWarning($"Object ID: {id} requested but not found");
        
        Debug.Log($"TriggerObjective firing with id: '{id}'");
        
        onObjectiveCompleted?.Invoke(id);
    }

    public bool IsObjectiveCompleted(string id)
    {
        return SaveManager.Instance != null && SaveManager.Instance.playerSaveData.completedObjectivesID.Contains(id);
    }
    
    // TODO DEBUG Delete later
    [SerializeField] private string debugObjectiveId;

    [ContextMenu("Trigger Debug Objective")]
    private void TriggerDebugObjective()
    {
        TriggerObjective(debugObjectiveId);
    }
}
