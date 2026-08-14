using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeypadInteract : Interactable
{
    [SerializeField] private CodeDoor codeDoor; // Reference to the door that the keypad unlocks
    
    [Header("UI References")]
    [SerializeField] private GameObject keypadPanel;

    [SerializeField] private TextMeshProUGUI displayText;

    [SerializeField] private TextMeshProUGUI feedbackText;

    [SerializeField] private Button enterButton;

    [SerializeField] private Button clearButton;

    [SerializeField] private Button closeButton;

    private const int CODE_LENGTH = 4;

    private string enteredCode = "";

    protected override void Interact()
    {
        OpenKeypad();
    }

    private void OpenKeypad()
    {
        enteredCode = "";
        UpdateDisplay();

        if (feedbackText != null)
            feedbackText.text = "";
        
        keypadPanel.SetActive(true);

        SetupButton();

        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    private void UpdateDisplay()
    {
        if (displayText != null)
            displayText.text = enteredCode.PadRight(CODE_LENGTH, '_');
    }
    
    private void SetupButton()
    {
        enterButton.onClick.RemoveAllListeners();
        enterButton.onClick.AddListener(SubmitCode);
        
        clearButton.onClick.RemoveAllListeners();
        clearButton.onClick.AddListener(ClearCode);
        
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(CloseKeypad);
    }

    public void AddDigit(string digit)
    {
        if (enteredCode.Length >= CODE_LENGTH)
            return;

        enteredCode += digit;
        UpdateDisplay();
    }
    
    private void SubmitCode()
    {
        if (codeDoor == null)
            return;

        bool success = codeDoor.TrySubmitCode(enteredCode);
        
        if(success)
            CloseKeypad();

        else
        {
            if (feedbackText != null)
                feedbackText.text = "Incorrect Code";

            enteredCode = "";
            UpdateDisplay();
        }
    }
    
    private void ClearCode()
    {
        enteredCode = "";
        UpdateDisplay();
    }
    
    private void CloseKeypad()
    {
        keypadPanel.SetActive(false);

        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
