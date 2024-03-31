using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class SmartSkeleton : MonoBehaviour, ICharacter
{
    bool IsMoving { 
        set {
            isMoving = value;
            animator.SetBool("isMoving", isMoving);
        }
    }
   public float hitDelay; 
   public bool isAgro; 
   public float minDistanceToPlayer = 2.6f; 
    public bool canMove; 
    Animator animator;
    SpriteRenderer spriteRenderer;
    public float damage = 1;
    public float knockbackForce = 20f;
    public float nextWaypointDistance = 3f; 
    private AIDestinationSetter destinationSetter;
    private AILerp aiLerp; 
    public Transform target; 
    public bool playerInRange; 
    Seeker seeker; 

    public AttackZone attackZone;
    Rigidbody2D rb;
    DamageableCharacter damagableCharacter;
    bool isMoving = false;

    void Start(){
        destinationSetter = GetComponent<AIDestinationSetter>();
        destinationSetter.target = target;
        attackZone = GetComponentInChildren<AttackZone>(); 
        if (attackZone == null){
            Debug.Log("Attack zone not detected on patrol skeleton");
        }
        canMove = true; 
        isMoving = false;
        rb = GetComponent<Rigidbody2D>();
        aiLerp = GetComponent<AILerp>();
        damagableCharacter = GetComponent<DamageableCharacter>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        seeker = GetComponent<Seeker>();
        isAgro = false; 
    }
     
    public void OnFreeze(bool isFrozen){
        if (isFrozen){
            LockMovement(); 
        }
        else {
            UnlockMovement(); 
        }
    }

    void moveOnAgro(){
        bool shouldFaceRight = target.position.x > transform.position.x;
        spriteRenderer.flipX = !shouldFaceRight;
        gameObject.BroadcastMessage("IsFacingRight", shouldFaceRight);
        
        float distanceToPlayer = Vector2.Distance(rb.position, target.position);
        if (distanceToPlayer <= minDistanceToPlayer) {
            // The boss is close to the player and not moving significantly
            IsMoving = false; 
        }
        else {
            IsMoving = true; 
        }
    }

    void FixedUpdate() {
        if (attackZone.playerDetected){
            isAgro = true; 
        }
        if (canMove && isAgro){
            moveOnAgro(); 
        }
        else {
            IsMoving = false;
        }
    }
    

    /// Deal damage and knockback to IDamageable 
    void OnCollisionEnter2D(Collision2D collision) {
        Collider2D collider = collision.collider;
        IDamageable damageable = collider.GetComponent<IDamageable>();

        if(damageable != null && collision.gameObject.tag != "Skeleton") {
            // Offset for collision detection changes the direction where the force comes from
            Vector2 direction = (collider.transform.position - transform.position).normalized;

            // Knockback is in direction of swordCollider towards collider
            Vector2 knockback = direction * knockbackForce;

            // After making sure the collider has a script that implements IDamagable, we can run the OnHit implementation and pass our Vector2 force
            damageable.OnHit(damage, knockback);
        }
    }

    public void LockMovement() {
        aiLerp.canMove = false; 
    }

    public void UnlockMovement() {
        canMove = true;
    }

    public IEnumerator ApplyKnockbackWithDelay(Vector2 knockback) {
         
         // aiLerp.speed = 0; 
    aiLerp.canMove = false;
    rb.AddForce(knockback, ForceMode2D.Impulse);

    yield return new WaitForSeconds(hitDelay); // Wait for knockback effect to apply

    aiLerp.Teleport(transform.position, true); // Update AILerp's position to the current position
    aiLerp.canMove = true;
        
    }
}
