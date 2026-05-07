using System;
using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public List<ItemData> items = new List<ItemData>();

    [Header("UI")]
    [SerializeField] public GameObject inventoryPanel;

    [SerializeField] private InventoryUI uiScript;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        
        inventoryPanel.SetActive(false);
    }


    public void ToggleInventory()
    {
        bool isOpen = !inventoryPanel.activeSelf;
        inventoryPanel.SetActive(isOpen);

        if (isOpen)
        {
            uiScript.RefreshUI();
            
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
        else
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void AddItem(ItemData item)
    {
        items.Add(item);
    }
    
}
