using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Skeleton : MonoBehaviour, ICharacter
{
    bool IsMoving { 
        set {
            isMoving = value;
            animator.SetBool("isMoving", isMoving);
        }
    }
    public bool canMove; 
    Animator animator;
    SpriteRenderer spriteRenderer;
    public float damage = 1;
    public float knockbackForce = 20f;
    public float moveSpeed = 500f;
    Vector3 currentPosition; 
    Vector3 lastPosition; 

    public DetectionZone detectionZone;
    Rigidbody2D rb;
    Vector2 moveInput = Vector2.zero;
    DamageableCharacter damagableCharacter;
    bool isMoving = false;

    void Awake(){
        Events.OnCharacterFreeze += OnFreeze;
    }

    void Start(){
        canMove = true; 
        lastPosition = transform.position; 
        rb = GetComponent<Rigidbody2D>();
        damagableCharacter = GetComponent<DamageableCharacter>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnFreeze(bool isFrozen){
        if (isFrozen){
            LockMovement(); 
        }
        else {
            UnlockMovement(); 
        }
    }

    void FixedUpdate() {

        if(canMove && damagableCharacter.Targetable && detectionZone.detectedObjs.Count > 0) {
            // Calculate direction to target object
            Vector2 direction = (detectionZone.detectedObjs[0].transform.position - transform.position).normalized;

            // Move towards detected object
            rb.AddForce(direction * moveSpeed * Time.fixedDeltaTime);

             if ((transform.position.x - lastPosition.x )> 0) {
                spriteRenderer.flipX = false;
                //gameObject.BroadcastMessage("IsFacingRight", true);
            } else if ((transform.position.x - lastPosition.x) < 0) {
                spriteRenderer.flipX = true;
                //gameObject.BroadcastMessage("IsFacingRight", false);
            }
            IsMoving = true;

        } else {
            IsMoving = false;
        }
       lastPosition = transform.position; 
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

    public void LockMovement() {
        canMove = false;
    }

    public void UnlockMovement() {
        canMove = true;
    }
}
