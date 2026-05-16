using UnityEngine;
using System.Collections;
using TMPro;

public class SaveInteract : Interactable
{
    [Header("UI Game saved Notification")]
    [SerializeField] private TextMeshProUGUI saveText;

    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float displayDuration = 2.0f;

    private Coroutine fadeCoroutine;
    
    protected override void Interact()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null && SaveManager.Instance != null)
        {
            SaveManager.Instance.SaveGame(player.transform.position);
            Debug.Log("Game saved successfully");

            // Making the game saved notification fade in and out
            if (saveText != null)
            {
                if(fadeCoroutine != null)
                    StopCoroutine(fadeCoroutine);

                fadeCoroutine = StartCoroutine(FadeNotificationLoop());
            }
        }

        else
            Debug.Log("Missing player object or SaveManager instance");
        
        
    }

    private IEnumerator FadeNotificationLoop()
    {
        // Fading the text in
        float counter = 0;
        Color originalColor = saveText.color;

        while (counter < fadeDuration)
        {
            counter += Time.unscaledDeltaTime; // Using unscaledDeltaTime so the text still fades in and out if the game is paused

            float alpha = Mathf.Lerp(01, 1f, counter / fadeDuration);

            saveText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }
        
        // Keeping the text on screen
        yield return new WaitForSeconds(displayDuration);
        
        // Fading the text out
        counter = 0;

        while (counter < fadeDuration)
        {
            counter += Time.unscaledDeltaTime;

            float alpha = Mathf.Lerp(1f, 0f, counter / fadeDuration);
            
            saveText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            yield return null;
        }
    }
}
