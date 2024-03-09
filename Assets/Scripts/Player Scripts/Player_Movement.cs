using UnityEngine;
using System.Collections;

public class Motion_Controller : MonoBehaviour {
    private float maxspeed; // Walk speed
    Animator anim;
    private bool faceright; // Face side of sprite activated
    private bool jumping = false;
    private bool isdead = false;

    void Start() {
        maxspeed = 2f; // Set walk speed
        faceright = true; // Default right side
        anim = GetComponent<Animator>();
        anim.SetBool("walk", false); // Walking animation is deactivated
        anim.SetBool("dead", false); // Dying animation is deactivated
        anim.SetBool("jump", false); // Jumping animation is deactivated
    }

    void OnCollisionEnter2D(Collision2D coll) {
        jumping = false;
        anim.SetBool("jump", false);
    }

    void Update() {
        if (!isdead) {
            // Dying
            if (Input.GetKey("k")) {
                anim.SetBool("dead", true);
                isdead = true;
            }

            // Jumping
            if (Input.GetButtonDown("Jump") && !jumping) {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 200));
                jumping = true;
                anim.SetBool("jump", true);
            }

            // Walking
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            GetComponent<Rigidbody2D>().velocity = new Vector2(moveHorizontal * maxspeed, moveVertical * maxspeed);

            if (moveHorizontal > 0) { // Go right
                anim.SetBool("walk", true);
                if (!faceright) {
                    Flip();
                }
            } else if (moveHorizontal < 0) { // Go left
                anim.SetBool("walk", true);
                if (faceright) {
                    Flip();
                }
            } else if (moveVertical > 0) { // Go up
                anim.SetBool("walk", true);
                // Handle flipping if needed for vertical movement
            } else if (moveVertical < 0) { // Go down
                anim.SetBool("walk", true);
                // Handle flipping if needed for vertical movement
            } else { // Stop
                anim.SetBool("walk", false);
            }

            // Swing sword
            if (Input.GetMouseButtonDown(0)) {
                anim.SetBool("attack", true);
            } else {
                anim.SetBool("attack", false);
            }
        }
    }

    void Flip() {
        faceright = !faceright;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
