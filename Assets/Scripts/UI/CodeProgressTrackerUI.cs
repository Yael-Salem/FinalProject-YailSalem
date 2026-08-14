using UnityEngine;
using TMPro;

public class CodeProgressTrackerUI : MonoBehaviour
{
    [SerializeField] private string codeObjectiveId = "find_code"; // The objective ID needed to display the code tracker UI

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI codeProgressText;

    private static string foundDigits = "----";

    public static void RecordDigit(int position, char value)
    {
        char[] chars = foundDigits.ToCharArray();
        chars[position] = value;

        foundDigits = new string(chars);
    }

    public void RefreshDisplay()
    {
        // Checking if the relevant objective is active
        bool isCodeObjectiveActive = ObjectiveManager.Instance != null &&
                               ObjectiveManager.Instance.currentObjectiveId == codeObjectiveId;

        if (codeProgressText == null)
            return;
        
        codeProgressText.gameObject.SetActive(isCodeObjectiveActive);

        if (isCodeObjectiveActive)
            codeProgressText.text = foundDigits;
    }
}
