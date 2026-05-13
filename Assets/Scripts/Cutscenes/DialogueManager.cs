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
    public string scriptFileName = "script.json";

    private Dictionary<string, Dialogue> dialogueFromFile = new Dictionary<string, Dialogue>();
    private Queue<string> sentences = new Queue<string>();
    private bool isTyping = false;
    private string currentSentence;
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
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, scriptFileName);

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

    public void StartDialogue(string dialogueId, System.Action onComplete)
    {
        if (!dialogueFromFile.TryGetValue(dialogueId, out Dialogue dialogue))
        {
            Debug.LogError($"Requested dialogue not found: {dialogueId}");
            onComplete?.Invoke();
            return;
        }

        onDialogueCompleteCallback = onComplete;
        dialogueBoxPanel.SetActive(true);

        if (string.IsNullOrEmpty(dialogue.speakerName))
            speakerBoxPanel.SetActive(false);

        else
        {
            speakerBoxPanel.SetActive(true);
            speakerNameText.text = dialogue.speakerName;
        }
        
        sentences.Clear();
        
        foreach(string sentence in dialogue.sentences)
            sentences.Enqueue(sentence);

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.text = currentSentence;
            isTyping = false;
            return;
        }

        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        currentSentence = sentences.Dequeue();
        StartCoroutine(TypeSentence(currentSentence));
    }
    
    private void EndDialogue()
    {
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
