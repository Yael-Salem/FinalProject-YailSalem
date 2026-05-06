using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Note : Interactable
{
    [SerializeField]
    [TextArea(10,20)]
    private string noteContent;

    [Header("UI References")]
    [SerializeField] private GameObject notePanel;
    [SerializeField] private TextMeshProUGUI displayText;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject prevButton;
    [SerializeField] private GameObject closeButton;

    private int currentPage = 0;
    
    protected override void Interact()
    {
        displayText.text = noteContent;
        
        notePanel.SetActive(true);
        
        // resetting then adding onClick listeners for each button through the code rather than having to set it manually through the editor for each note
        nextButton.GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
        nextButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(NextPage);
        
        prevButton.GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
        prevButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(PrevPage);
        
        closeButton.GetComponent<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
        closeButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(CloseNote);
        
        Canvas.ForceUpdateCanvases();
        UpdatePageDisplay();

        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseNote()
    {
        notePanel.SetActive(false);
        
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
}

