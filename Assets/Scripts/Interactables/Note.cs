using System;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Note : Interactable
{
    [Header("Save data")] 
    [ContextMenuItem("Generate New ID", "GenerateSaveID")]
    [SerializeField] private string noteId;
    
    
    [SerializeField]
    private ItemData itemData;

    [Header("UI References")]
    [SerializeField] private GameObject notePanel;
    [SerializeField] private TextMeshProUGUI displayText;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject prevButton;
    [SerializeField] private GameObject closeButton;

    private int currentPage = 0;

    protected override void Interact()
    {
        if (itemData == null)
            return;
        
        OpenNote(itemData);
    }

    private void Start()
    {
        // Checking if the note has already been collected and destroying it right away if it has
        if(SaveManager.Instance != null && SaveManager.Instance.playerSaveData.collectiblesIDs.Contains(noteId))
            Destroy(gameObject);
    }


    protected void OpenNote(ItemData data)
    {
        itemData = data;
        
        displayText.text = itemData.fullNoteContent;
        
        notePanel.SetActive(true);
       
        SetupButtons();
        
        Canvas.ForceUpdateCanvases();
        UpdatePageDisplay();

        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    // Function to reset and add listeners to all the buttons
    private void SetupButtons()
    {
        nextButton.GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
        nextButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(NextPage);
        
        prevButton.GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
        prevButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(PrevPage);
        
        closeButton.GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
        closeButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(CloseNote);
    }

    public void CloseNote()
    {
        notePanel.SetActive(false);

        // Checking if we opened the note from the inventory UI
        if (GetComponent<RectTransform>() != null)
        {
            InventoryManager.Instance.inventoryPanel.SetActive(true);
        }
        
        else
        {
            if(SaveManager.Instance != null && !string.IsNullOrEmpty(noteId))
                SaveManager.Instance.playerSaveData.collectiblesIDs.Add(noteId);
            
            
            InventoryManager.Instance.AddItem(itemData);
            
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            Destroy(gameObject);
        }
        
    }

    public void NextPage()
    {
        if (currentPage < displayText.textInfo.pageCount - 1)
        {
            currentPage++;
            UpdatePageDisplay();
        }
    }

    public void PrevPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdatePageDisplay();
        }
    }

    public void UpdatePageDisplay()
    {
        displayText.pageToDisplay = currentPage + 1;
        
        nextButton.SetActive(currentPage < displayText.textInfo.pageCount - 1);
        prevButton.SetActive(currentPage > 0);
    }

    public void OpenFromInventory(ItemData data)
    {
        itemData = data;
        
        Interact();
    }

    private void GenerateSaveID()
    {
        noteId = System.Guid.NewGuid().ToString();
    }
}

