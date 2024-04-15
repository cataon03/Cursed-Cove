using System.Collections;
using System.Collections.Generic;
using Yarn.Unity; 
using Cinemachine; 
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    public float cinematicDuration = 5f; // Adjust this value based on the desired cinematic duration
    private CinemachineBrain cinemachineBrain; 

    public CinemachineVirtualCamera[] cameras;
    
    private CinemachineVirtualCamera currentCam;
    private CinemachineVirtualCamera lastCam; 
    
    private bool checkAndReset = false;  


    private void Awake(){
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        cinemachineBrain = FindObjectOfType<CinemachineBrain>(); 
    }

    void Update(){
        if (checkAndReset && cinemachineBrain.ActiveVirtualCamera != null){
            lastCam.enabled = false; 
            currentCam.enabled = true;
            checkAndReset = false; 
        }
    }

    public void SwitchToCamera(CinemachineVirtualCamera secondaryCamera){
        checkAndReset = true; 
        currentCam = secondaryCamera; 
        lastCam = secondaryCamera; 
    }

    [YarnCommand("switch_cam")]
    public void switchToCamera(int cameraIndex){
        SwitchToCamera(cameras[cameraIndex]);
    }
    
}