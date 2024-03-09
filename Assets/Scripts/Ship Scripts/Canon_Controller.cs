using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


/*
public class Canon_Controller : MonoBehaviour
{
    //Variables for canon
    public Transform canonBarrel;
    public CinemachineVirtualCamera cameraFollowingShip;

    // Update is called once per frame
    void Update()
    {
        //Check for mouse click
        if (Input.GetMouseButtonDown(0))
        {
            //Raycast to get the position of the mouse click using the cinemachine camera
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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
*/

public class CannonScript : MonoBehaviour
{
    public GameObject projectilePrefab; // Prefab of the non-violent projectile
    //public float projectileSpeed; // Speed of the projectile
    public Transform barrel; // Transform of the cannon barrel

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Check if player is on ship and clicks
        {
            // Get the mouse position in world coordinates
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Calculate the direction vector from the cannon to the clicked position
            Vector3 direction = (mousePosition - transform.position).normalized;

            // Rotate the cannon barrel towards the clicked position
            RotateBarrel(direction);

            // Shoot the projectile based on the clicked position and direction
            //ShootProjectile(direction);
        }
    }

    void RotateBarrel(Vector3 direction)
    {
        // Calculate the angle to rotate the barrel
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Apply the rotation to the cannon barrel
        barrel.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}

