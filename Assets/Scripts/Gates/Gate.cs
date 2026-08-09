using System.Collections;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private float openY = 4.36f;
    [SerializeField] private float closedY = -2.057869f;
    [SerializeField] private float moveSpeed = 10f;
    
    [SerializeField] private float delaySeconds = 7f; // How many seconds to wait before closing the gate
    
    // Audio source and sound effect files
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip closeSound;
    [SerializeField] private AudioClip openSound;

    private Coroutine moveCoroutine;
    
    public void Close()
    {
        if(moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(MoveToY(closedY));
        
        if(audioSource != null && closeSound != null)
            audioSource.PlayOneShot(closeSound);
    }
    
    public void Open()
    {
        if(moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(MoveToY(openY));
        
        if(audioSource != null && openSound != null)
            audioSource.PlayOneShot(openSound);
    }
    
    // Getter for delaySeconds
    public float GetDelaySeconds() => delaySeconds;
    
    private IEnumerator MoveToY(float targetY)
    {
        while (Mathf.Abs(transform.position.y - targetY) > 0.01f)
        {
            Vector3 pos = transform.position;

            pos.y = Mathf.MoveTowards(pos.y, targetY, moveSpeed * Time.deltaTime);
            
            transform.position = pos;

            yield return null;
        }
    }
}
