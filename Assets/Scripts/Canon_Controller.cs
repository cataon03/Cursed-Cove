using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Canon_Controller : MonoBehaviour
{
    //Variables for canon
    public Transform canonBarrel;
    public CinemachineVirtualCamera cameraFollowingShip;
    public Camera camera;

    //public GameObject canonBall;

    // Update is called once per frame
    void Update()
    {
        //Check for mouse click
        if (Input.GetMouseButtonDown(0))
        {
            //Raycast to get the position of the mouse click
            Ray ray = camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //Get the direction of the mouse click
                Vector3 direction = hit.point - canonBarrel.position;
                direction.y = 0f;

                //Rotate the canon barrel to face the direction of the mouse click
                canonBarrel.rotation = Quaternion.LookRotation(direction);

                //Shoot the canon ball
            }
        }
        
    }
}
