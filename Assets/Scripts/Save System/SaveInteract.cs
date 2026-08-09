using UnityEngine;
using System.Collections;

public class SaveInteract : Interactable
{
    protected override void Interact()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null && SaveManager.Instance != null)
        {
            SaveManager.Instance.SaveGame(player.transform.position);
            Debug.Log("Game saved successfully");
        }

        else
            Debug.Log("Missing player object or SaveManager instance");
        
        
    }
}
