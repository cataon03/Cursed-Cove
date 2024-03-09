using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

// Loads a new level when the tagTarget walks onto the triggering collider
public class LevelTransition : MonoBehaviour
{
    public string tagTarget = "Player";
    public SceneAsset sceneToLoad;


    void Start(){
        Debug.Log("Level Transition Start");
    }

    void OnTriggerEnter2D(Collider2D collider) {
        Debug.Log("Collision");

        if(collider.gameObject.tag == tagTarget) {
            // Tag Target walked onto collider shape so transition level
            SceneManager.LoadSceneAsync(sceneToLoad.name, LoadSceneMode.Single);
        }
    }
}
