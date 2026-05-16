using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    // Player position
    public float[] playerPosition = new float[3];
    
    // Player's health at the time of saving
    public float playerHealth;
    
    // List of collectibles ID's the player has collected
    public List<string> collectiblesIDs = new List<string>();
    
    // List of cutscene ID's that the player has already watched
    public List<string> watchedCutscenesID = new List<string>();
}



public class SaveManager : MonoBehaviour
{
    // Using a singleton pattern
    public static SaveManager Instance { get; private set; }
    
    
    public SaveData playerSaveData = new SaveData();
    private string saveFilePath;
    
    // Item Database to use in order to repopulate player's inventory
    [SerializeField] private List<ItemData> allGameItems = new List<ItemData>();

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

        saveFilePath = System.IO.Path.Combine(Application.persistentDataPath, "gamesave.json");
        
        allGameItems = new List<ItemData>(Resources.LoadAll<ItemData>("")); // Loading items automatically
        Debug.Log($"Successfully loaded {allGameItems.Count} items from Resources folder");
    }

    public void SaveGame(Vector3 currentPlayerPosition)
    {
        // Saving player's current position
        playerSaveData.playerPosition[0] = currentPlayerPosition.x;
        playerSaveData.playerPosition[1] = currentPlayerPosition.y;
        playerSaveData.playerPosition[2] = currentPlayerPosition.z;
        
        // Saving player's current health
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        if (player != null && player.TryGetComponent<PlayerHealth>(out var playerHealth))
            playerSaveData.playerHealth = playerHealth.Health;

        string json = JsonUtility.ToJson(playerSaveData, true);
        
        File.WriteAllText(saveFilePath, json);
        Debug.Log($"Saved Successfully to {saveFilePath}");
    }

    public bool LoadGame()
    {
        if (!File.Exists(saveFilePath))
        {
            Debug.LogWarning("Save file not found");
            return false;
        }

        string json = File.ReadAllText(saveFilePath);

        playerSaveData = JsonUtility.FromJson<SaveData>(json);

        TeleportPlayer();
        RepopulateInventory();
        LoadPlayerHealth();

        Debug.Log("Save data loaded successfully");
        return true;
    }

    private void LoadPlayerHealth()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        if (player != null && player.TryGetComponent<PlayerHealth>(out var playerHealth))
            playerHealth.SetCurrentHealth(playerSaveData.playerHealth);
    }

    private void RepopulateInventory()
    {
        if (InventoryManager.Instance == null)
            return;
        
        InventoryManager.Instance.ClearInventory();
        
        
        foreach (string itemID in playerSaveData.collectiblesIDs)
        {
            ItemData matchedItem = allGameItems.Find(item => item.name == itemID || item.itemName == itemID);
            
            if(matchedItem != null)
                InventoryManager.Instance.AddItem(matchedItem);
            
            else
                Debug.LogWarning($"Could not find item {itemID}");
        }
        
        
    }

    private void TeleportPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
            return;
        
        // Getting player position data from our array
        float playerXPosition = playerSaveData.playerPosition[0];
        float playerYPosition = playerSaveData.playerPosition[1];
        float playerZPosition = playerSaveData.playerPosition[2];

        Vector3 targetPosition = new Vector3(playerXPosition, playerYPosition, playerZPosition);
        
        // Disabling the character controller while we teleport the player as to not interfere with the teleportation
        if (player.TryGetComponent<CharacterController>(out var controller))
        {
            controller.enabled = false;
            player.transform.position = targetPosition;
            controller.enabled = true;
        }

        else
            player.transform.position = targetPosition;
    }
}
