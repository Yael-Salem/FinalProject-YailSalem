using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    // Using a singleton pattern
    public static DialogueManager Instance { get; private set; }

    [Header("UI Elements")]
    public GameObject dialogueBoxPanel;
    public GameObject speakerBoxPanel;
    public Button skipButton;

    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI speakerNameText;

    public float typingSpeed = 0.02f;
    
    // File where we will be reading the text from
    private const string SCRIPT_FILE_NAME = "script.json";

    private Dictionary<string, Dialogue> dialogueFromFile = new Dictionary<string, Dialogue>();
    private Queue<DialogueLine> sentences = new Queue<DialogueLine>();
    private bool isTyping = false;
    private DialogueLine currentData;
    private string currentSentence;
    private PlayerLook playerLookController; // Controlling the player's head movement if we need them to look a certain way
    private PlayerMotor playerMotorController; // Reference to the motor system in case we need to move the player to a specific target during the cutscene
    private System.Action onDialogueCompleteCallback;
    private Coroutine typingCoroutine;
    private Coroutine moveCoroutine; // Keeping track of active cutscene movement loop
    private string currentActiveDialogueId;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        else
            Destroy(gameObject);

        dialogueBoxPanel.SetActive(false);

        if (skipButton != null)
        {
            skipButton.gameObject.SetActive(false);
            skipButton.onClick.AddListener(SkipCutscene);
            
        }
            
        
        LoadDialogueData();
    }

    private void LoadDialogueData()
    {
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, SCRIPT_FILE_NAME);

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

        currentActiveDialogueId = dialogueId;

        if (player != null)
        {
            player.TryGetComponent<PlayerLook>(out playerLookController);
            player.TryGetComponent<PlayerMotor>(out playerMotorController);
        }
            

        onDialogueCompleteCallback = onComplete;

        if (dialogueText != null)
            dialogueText.text = string.Empty;

        if (speakerNameText != null)
            speakerNameText.text = string.Empty;
        
        dialogueBoxPanel.SetActive(true);
        
        if(skipButton != null)
            skipButton.gameObject.SetActive(true);
        
        sentences.Clear();
        
        foreach(DialogueLine line in dialogue.lines)
            sentences.Enqueue(line);

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (isTyping)
        {
            if(typingCoroutine != null)
                StopCoroutine(typingCoroutine);
            
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

        if (playerMotorController != null && !string.IsNullOrEmpty(currentData.moveTargetTag))
        {
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
                moveCoroutine = null;
            }
            
            GameObject foundTarget = GameObject.FindWithTag(currentData.moveTargetTag);

            if (foundTarget != null)
                moveCoroutine = StartCoroutine(CutsceneMoveLoop(foundTarget.transform));
        }
        
        
        typingCoroutine = StartCoroutine(TypeSentence(currentSentence));
    }

    private IEnumerator CutsceneMoveLoop(Transform destination)
    {
        if(destination == null || playerMotorController == null)
            yield break;

        // Getting player's current position
        Transform playerTransform = playerMotorController.transform;

        // Moving the player until we reach just before the destination
        while (Vector3.SqrMagnitude(
                   new Vector3(destination.position.x, playerTransform.position.y, destination.position.z) -
                   playerTransform.position) > 5.0f)
        {
            playerMotorController.MoveTowardsTarget(destination);
            yield return null;
        }

        moveCoroutine = null;
    }

    public void SkipCutscene()
    {
        if(typingCoroutine != null)
            StopCoroutine(typingCoroutine);
        
        sentences.Clear();
        EndDialogue();
    }
    
    private void EndDialogue()
    {
        if(playerLookController != null)
            playerLookController.ClearCutsceneLookTarget();
        
        // Making the player stop tracking targets when the cutscene ends
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
        
        dialogueBoxPanel.SetActive(false);
        if(skipButton != null)
            skipButton.gameObject.SetActive(false);

        if (!string.IsNullOrEmpty(currentActiveDialogueId) &&
            dialogueFromFile.TryGetValue(currentActiveDialogueId, out Dialogue completedDialogue))
        {
            if (!string.IsNullOrEmpty(completedDialogue.triggersObjectiveId))
            {
                ObjectiveManager.Instance.TriggerObjective(completedDialogue.triggersObjectiveId);
            }
        }

        currentActiveDialogueId = null;
        
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
