using System;
using UnityEngine;

public class AreaCutsceneTrigger : MonoBehaviour
{
    private string cutsceneId;
    private System.Action onCompleteCallback;
    private GameObject playerRef;
    
    public void SetupTrigger(string id, GameObject player, Action onComplete)
    {
        cutsceneId = id;
        playerRef = player;
        onCompleteCallback = onComplete;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == playerRef)
        {
            if (TryGetComponent<Collider>(out var collider))
                collider.enabled = false;
            
            DialogueManager.Instance.StartDialogue(cutsceneId, playerRef, () =>
            {
                onCompleteCallback?.Invoke();
                
                Destroy(gameObject);
            });
        }
    }
}
