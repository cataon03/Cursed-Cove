using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Include SceneManagement namespace

public class SceneSwitcher : MonoBehaviour
{
    //Name of scene to load and set switch to false
    public string sceneToLoad = "Ship Sail Scene"; // Name of the scene you want to load
    private bool canSwitchScene = false;

    //When player enters the dock, they can switch scenes
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            canSwitchScene = true;
            Debug.Log("Player has reached the dock. Press 'E' to switch scenes.");
        }
    }

    //When player exits the dock, they can no longer switch scenes
    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            canSwitchScene = false;
        }
    }

    //If player presses 'E', load the scene
    private void Update() {
        if (canSwitchScene && Input.GetKeyDown(KeyCode.E)) {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

}
