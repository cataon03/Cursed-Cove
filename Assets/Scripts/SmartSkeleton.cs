using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Pathfinding; 

public class SmartSkeleton : MonoBehaviour 

{ public bool canMove = true; 
    Animator animator;
    SpriteRenderer spriteRenderer;
    public float hitDelay; 
    public float damage = 1;
    public float knockbackForce = 20f;
    Rigidbody2D rb;
    public AIPath aiPath; 

    void Awake(){
        Events.OnCharacterFreeze += OnFreeze;
    }

    void Start(){
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        aiPath = GetComponentInParent<AIPath>(); // Assuming the AIPath component is on the parent object
    }

    public void OnFreeze(bool isFrozen){
        if (isFrozen){
            LockMovement(); 
        }
        else {
            UnlockMovement(); 
        }
    }

    void Update() {
        if(canMove) {
            // Use velocity.magnitude to check if the skeleton is moving
            bool isMoving = aiPath.velocity.magnitude > 0.1; // Adjust the threshold as needed
            animator.SetBool("isMoving", isMoving);

            // Update sprite direction based on velocity
            if (aiPath.velocity.x > 0.1) {
                spriteRenderer.flipX = false;
            } else if (aiPath.velocity.x < -0.1) {
                spriteRenderer.flipX = true;
            }
        }
    }
    
    /// Deal damage and knockback to IDamageable 
    void OnCollisionEnter2D(Collision2D collision) {
        Collider2D collider = collision.collider;
        IDamageable damageable = collider.GetComponent<IDamageable>();

        if (damageable != null && collision.gameObject.tag != "Skeleton") {
            // Offset for collision detection changes the direction where the force comes from
            Vector2 direction = (collider.transform.position - transform.position).normalized;

            // Knockback is in direction of swordCollider towards collider
            Vector2 knockback = direction * knockbackForce;

            // After making sure the collider has a script that implements IDamagable, we can run the OnHit implementation and pass our Vector2 force
            damageable.OnHit(damage, knockback);
        }
    }

    public IEnumerator ApplyKnockbackWithDelay(Vector2 knockback) {
        if (aiPath != null) {
            aiPath.enabled = false; // Disable pathfinding movement
            LockMovement(); 
            rb.velocity = knockback; // Apply knockback
            yield return new WaitForSeconds(hitDelay); // Wait for knockback effect to apply
            UnlockMovement(); 
            aiPath.enabled = true; // Re-enable pathfinding movement
        }
    }

    public void LockMovement() {
        canMove = false;
        animator.SetBool("isMoving", false); 
    }

    public void UnlockMovement() {
        canMove = true;
        animator.SetBool("isMoving", true); 
    }
}