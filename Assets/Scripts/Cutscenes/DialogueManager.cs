using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    // Using a singleton pattern
    public static DialogueManager Instance { get; private set; }

    [Header("UI Elements")]
    public GameObject dialogueBoxPanel;
    public GameObject speakerBoxPanel;

    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI speakerNameText;

    public float typingSpeed = 0.02f;
    
    // File where we will be reading the text from
    private const string ScriptFileName = "script.json";

    private Dictionary<string, Dialogue> dialogueFromFile = new Dictionary<string, Dialogue>();
    private Queue<DialogueLine> sentences = new Queue<DialogueLine>();
    private bool isTyping = false;
    private DialogueLine currentData;
    private string currentSentence;
    private PlayerLook playerLookController; // Controlling the player's head movement if we need them to look a certain way
    private System.Action onDialogueCompleteCallback;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        else
            Destroy(gameObject);

        dialogueBoxPanel.SetActive(false);
        LoadDialogueData();
    }

    private void LoadDialogueData()
    {
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, ScriptFileName);

        if (File.Exists(filePath))
        {
            string rawJson = File.ReadAllText(filePath);

            DialogueDatabase db = JsonUtility.FromJson<DialogueDatabase>(rawJson);
            
            dialogueFromFile.Clear();

            foreach (Dialogue dialogue in db.cutscenes)
            {
                if(!dialogueFromFile.ContainsKey(dialogue.id))
                    dialogueFromFile.Add(dialogue.id, dialogue);

                else
                    Debug.LogWarning($"Duplicate ID: {dialogue.id}");
            }
        }

        else
        {
            Debug.LogError("script.json file not found");
        }
    }

    public void StartDialogue(string dialogueId,GameObject player, System.Action onComplete)
    {
        if (!dialogueFromFile.TryGetValue(dialogueId, out Dialogue dialogue))
        {
            Debug.LogError($"Requested dialogue not found: {dialogueId}");
            onComplete?.Invoke();
            return;
        }

        if (player != null)
            player.TryGetComponent<PlayerLook>(out playerLookController);

        onDialogueCompleteCallback = onComplete;
        dialogueBoxPanel.SetActive(true);
        
        sentences.Clear();
        
        foreach(DialogueLine line in dialogue.lines)
            sentences.Enqueue(line);

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.text = currentData.text;
            isTyping = false;
            return;
        }

        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        currentData = sentences.Dequeue();
        currentSentence = currentData.text;

        if (string.IsNullOrEmpty(currentData.speaker))
            speakerBoxPanel.SetActive(false);

        else
        {
            speakerBoxPanel.SetActive(true);
            speakerNameText.text = currentData.speaker;
        }

        if (playerLookController != null)
        {
            if (!string.IsNullOrEmpty(currentData.lookTargetTag))
            {
                Transform target = GameObject.FindWithTag(currentData.lookTargetTag)?.transform;
                playerLookController.SetCutsceneTrigger(target);
            }
        }
        
        StartCoroutine(TypeSentence(currentSentence));
    }
    
    private void EndDialogue()
    {
        if(playerLookController != null)
            playerLookController.ClearCutsceneLookTarget();
        
        dialogueBoxPanel.SetActive(false);
        onDialogueCompleteCallback?.Invoke();
    }
    
    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        isTyping = true;

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }
}
