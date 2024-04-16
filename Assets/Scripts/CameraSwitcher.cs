using System.Collections;
using System.Collections.Generic;
using Yarn.Unity; 
using Cinemachine; 
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public CinemachineVirtualCamera camera1;
    public CinemachineVirtualCamera camera2;
    
    [YarnCommand("switch_to_cam_1")]
    public void switchToCamera1(){
        CameraManager.instance.SwitchToCamera(camera1);
    }

    [YarnCommand("switch_to_cam_2")]
    public void switchToCamera2(){
        CameraManager.instance.SwitchToCamera(camera2);
    }
}