using System.Collections;
using System.Collections.Generic;
using Cinemachine; 
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public CinemachineVirtualCamera playerCamera;
    public CinemachineVirtualCamera cinematicCamera;

    public float cinematicDuration = 5f; // Adjust this value based on the desired cinematic duration
    private bool isCinematicActive = false;

    private void Start()
    {
        // Ensure the player camera is initially active, and the cinematic camera is inactive
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
    }
}