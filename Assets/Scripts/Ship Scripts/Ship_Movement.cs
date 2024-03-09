using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    //Speed of ship
    public float speed = 2;
    private Rigidbody2D rb;
    float currentSpeed;    
    
    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = 0;
    }

    // Update is called once per frame
    void Update() {

        //Ship movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(horizontal, vertical);

        rb.velocity = movement * currentSpeed;

        //If player presses w or up arrow, move up
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            rb.velocity = new Vector2(0, speed);

            //Change rotation of ship to up (x = -180 degrees)
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }

        //If player presses s or down arrow, move down
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            rb.velocity = new Vector2(0, -speed);

            //Change rotation of ship to down (x = 0 degrees)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        //If player presses a or left arrow, move left
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            rb.velocity = new Vector2(-speed, 0);

            //Change rotation of ship to left (x = -90 degrees)
            transform.rotation = Quaternion.Euler(0, 0, -90);
        }

        //If player presses d or right arrow, move right
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            rb.velocity = new Vector2(speed, 0);

            //Change rotation of ship to right (x = 90 degrees)
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }

        //Diagonal moves
        else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D)) {
            rb.velocity = new Vector2(speed, speed);
        }

        else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A)) {
            rb.velocity = new Vector2(-speed, speed);
        }

        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D)) {
            rb.velocity = new Vector2(speed, -speed);
        }

        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A)) {
            rb.velocity = new Vector2(-speed, -speed);
        }

        //Else remain idle
        else {
            rb.velocity = new Vector2(0, 0);
        }
    }
    
    public void Move(float horizontalInput, float verticalInput)
    {
        // Calculate movement direction based on input
        Vector3 movementDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // Apply movement to the ship
        transform.Translate(movementDirection * speed * Time.deltaTime);
    }
 
}