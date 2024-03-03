using System.Collections;
using System.Collections.Generic;
using Cinemachine; 
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public CinemachineVirtualCamera camera1;
    public CinemachineVirtualCamera camera2;
  

    public void switchToCamera1(){
        CameraManager.instance.SwitchToCamera(camera1);
    }

    public void switchToCamera2(){
        CameraManager.instance.SwitchToCamera(camera2);
    }

    public void toggleCameras(){
        /*
        if (isCam1Active){
            Debug.Log("switching cam position"); 
            CameraManager.instance.SwitchToCamera(camera2);
        }
        else {
            Debug.Log("switching back to cam1"); 
            CameraManager.instance.SwitchToCamera(camera1);
        }*/
    }

    //public float cinematicDuration = 5f; // Adjust this value based on the desired cinematic duration
    //private bool isCinematicActive = false;

/*
    public void SwitchCameras(cameraToActivate){
        if (cameraToActivate != null){
            CameraManager.instance.SwitchToCamera(cameraToActivate);
        }
        else {
            Debug.Log("CameraSwitcher has no camera set."); 
        }
    }
*/
/*
    private void Awake(){
        SetCameraState(cinematicCamera, false); 
    }
    
    private void Start()
    {
        // Ensure the player camera is initially active, and the cinematic camera is inactive
        //SetCameraState(playerCamera, true);
        //SetCameraState(cinematicCamera, false);
    }

    public void manualSwitchToCinematicCam(){
        SetCameraState(cinematicCamera, true); 
        SetCameraState(playerCamera, false); 
    }

    public void manualSwitchToPlayerCam(){
        SetCameraState(playerCamera, true);
        SetCameraState(cinematicCamera, false);
    }

    public void SwitchCameras()
    {

        StartCoroutine(SwitchToCinematicCamera());

    }

    IEnumerator SwitchToCinematicCamera()
    {
        // Set the flag to prevent multiple simultaneous cinematic switches
        isCinematicActive = true;

        // Disable the player camera and enable the cinematic camera
        SetCameraState(playerCamera, false);
        SetCameraState(cinematicCamera, true);

        // Wait for the cinematic duration
        yield return new WaitForSeconds(cinematicDuration);

        // Disable the cinematic camera and enable the player camera
        SetCameraState(cinematicCamera, false);
        SetCameraState(playerCamera, true);

        // Reset the flag
        isCinematicActive = false;
    }

    private void SetCameraState(CinemachineVirtualCamera camera, bool isActive)
    {
        if (camera != null)
        {
            camera.enabled = isActive;
        }
    } */ 

}