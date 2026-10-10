using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneTrigger : MonoBehaviour
{
    [SerializeField] private string sceneName = "MainMenu"; // Name of the scene we want to load, defaults to the main menu

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            SceneManager.LoadSceneAsync(sceneName);
    }
}
