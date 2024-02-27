using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcherCollider : MonoBehaviour
{
    [SerializeField]CameraSwitcher cameraSwitcher; 

    void OnTriggerEnter2D(Collider2D collider) {
        if (cameraSwitcher != null){
            cameraSwitcher.SwitchCameras(); 
        }
        else {
            Debug.Log("No CameraSwitcher object for the CameraSwitcherCollider"); 
        }
        GetComponent<Collider2D>().enabled = false; 
    }

}
