using System.Collections;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private float openY = 4.36f;
    [SerializeField] private float closedY = -5.87f;
    [SerializeField] private float moveSpeed = 10f;
    
    [SerializeField] private float delaySeconds = 7f; // How many seconds to wait before closing the gate

    private Coroutine moveCoroutine;
    
    public void Close()
    {
        if(moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(MoveToY(closedY));
    }
    
    public void Open()
    {
        if(moveCoroutine != null)
            StopCoroutine(moveCoroutine);

        moveCoroutine = StartCoroutine(MoveToY(openY));
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
