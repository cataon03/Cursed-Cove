using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    public string tagTarget = "Player";

    // When object is detected, it is added to the list of actively detected objects
    public List<Collider2D> detectedObjs = new List<Collider2D>();


    // Detect when object enters range
    void OnTriggerEnter2D(Collider2D collider) {
        if(collider.gameObject.tag == tagTarget) {
            detectedObjs.Add(collider);
        }
    }

    // Detect when object leaves range
    void OnTriggerExit2D(Collider2D collider) {
        if(collider.gameObject.tag == tagTarget) {
            detectedObjs.Remove(collider);
        }
    }
}
