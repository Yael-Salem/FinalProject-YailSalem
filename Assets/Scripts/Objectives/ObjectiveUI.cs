using UnityEngine;
using TMPro;

public class ObjectiveUI : MonoBehaviour
{
    [SerializeField] private GameObject objectiveHeaderText;
    [SerializeField] private TextMeshProUGUI objectiveTitleText;

    [SerializeField] private float displayDuration = 4.0f;

    private float hideTimer;
    
    void Awake()
    {
       if(objectiveHeaderText != null)
           objectiveHeaderText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (objectiveHeaderText != null && objectiveHeaderText.activeSelf)
        {
            hideTimer -= Time.deltaTime;
            
            if(hideTimer <= 0)
                objectiveHeaderText.SetActive(false);
        }
    }

    public void ShowNewObjective(string titleText)
    {
        if (objectiveHeaderText == null || objectiveTitleText == null)
        {
            Debug.LogError($"Missing Objective UI refrence");
            return;
        }

        objectiveTitleText.text = titleText;
        objectiveHeaderText.SetActive(true);
        hideTimer = displayDuration;
    }
}
