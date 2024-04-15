using System.Collections;
using System.Collections.Generic;
using System; 
using Yarn.Unity; 
using Cinemachine; 
using UnityEngine;

public class SkeletonBossTrigger : MonoBehaviour
{
    public static event Action OnReleaseBoss;  
    private DetectionZone detectionZone; 

    public void Start(){ 
        detectionZone = gameObject.GetComponentInChildren<DetectionZone>(); 
    }

    public void FixedUpdate(){
        if (detectionZone.detectedObjs.Count > 0){
            OnReleaseBoss?.Invoke();
            Destroy(gameObject); 
        }
    }
}