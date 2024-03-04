using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class BossController1 : MonoBehaviour, ICharacter
{
    bool IsMoving { 
        set {
            isMoving = value;
            animator.SetBool("isMoving", isMoving);
        }
    }

   // public AIPath aIPath; 
   public float minDistanceToPlayer = 2.6f; 
    public bool canMove; 
    Animator animator;
    bool isFlippedX = false; 
    SpriteRenderer spriteRenderer;
    public float damage = 1;
    public float knockbackForce = 20f;
    public float speed = 500f;
    public float nextWaypointDistance = 3f; 
    Vector3 currentPosition; 
    Vector3 lastPosition; 
    public Transform target; 
    Path path; 
    int currentWaypoint = 0; 
    bool reachedEndOfPath = false; 
    Seeker seeker; 

    public AttackZone attackZone;
    Rigidbody2D rb;
    Vector2 moveInput = Vector2.zero;
    DamageableCharacter damagableCharacter;
    bool isMoving = false;

    void Awake(){
        DialogueTriggerWithCollider.OnCharacterFreeze += OnFreeze;
    }

    void Start(){
        canMove = true; 
        isMoving = false;
        lastPosition = transform.position; 
        rb = GetComponent<Rigidbody2D>();
        damagableCharacter = GetComponent<DamageableCharacter>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        seeker = GetComponent<Seeker>();
        InvokeRepeating("UpdatePath", 0f, 0.5f); 
    }
     void UpdatePath(){
        if (seeker.IsDone()){
            seeker.StartPath(rb.position, target.position, OnPathComplete); 
        }
    }

    void OnPathComplete(Path p){
        if (!p.error){
            path = p; 
            currentWaypoint = 0; 
        } 
    }


    public void OnFreeze(bool isFrozen){
        if (isFrozen){
            Debug.Log("received freeze"); 
            LockMovement(); 
        }
        else {
            Debug.Log("received freeze"); 
            UnlockMovement(); 
        }
    }
    void Update(){
        
    }

    void FixedUpdate() {

        if (canMove) {
            float distanceToPlayer = Vector2.Distance(rb.position, target.position);
            if (distanceToPlayer <= minDistanceToPlayer) {
                // The boss is close to the player and not moving significantly
                IsMoving = false; 
            }
            else {
                IsMoving = true; 
            }

            if (path == null){
                return; 
            }
            if (currentWaypoint >= path.vectorPath.Count){
                reachedEndOfPath = true; 
                return; 
            }
            else {
                reachedEndOfPath = false; 
            }

            Vector2 direction = ((Vector2) path.vectorPath[currentWaypoint] - rb.position).normalized; 
            Vector2 force = direction * speed * Time.deltaTime; 
            
           float directionToTarget = target.position.x - rb.position.x;

        // Flip the sprite based on the direction to the target
        if (directionToTarget > 0) {
            spriteRenderer.flipX = false; // Assuming the sprite faces right by default
        } else if (directionToTarget < 0) {
            spriteRenderer.flipX = true;
        }
            rb.AddForce(force); 

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
            if (distance < nextWaypointDistance){
                currentWaypoint++; 
            }
            if (attackZone.detectedObjs.Count > 0){
                Debug.Log("attack!"); 
                animator.SetTrigger("attack");
            }
            
        } 
        else {
            IsMoving = false;
           
        }
       lastPosition = transform.position; 
    }
    

    /// Deal damage and knockback to IDamageable 
    void OnCollisionEnter2D(Collision2D collision) {
        Collider2D collider = collision.collider;
        IDamageable damageable = collider.GetComponent<IDamageable>();

        if(damageable != null) {
            // Offset for collision detection changes the direction where the force comes from
            Vector2 direction = (collider.transform.position - transform.position).normalized;

            // Knockback is in direction of swordCollider towards collider
            Vector2 knockback = direction * knockbackForce;

            // After making sure the collider has a script that implements IDamagable, we can run the OnHit implementation and pass our Vector2 force
            damageable.OnHit(damage, knockback);
        }
    }

    public void LockMovement() {
        Debug.Log("Position locked");
        canMove = false;
    }

    public void UnlockMovement() {
        Debug.Log("Position unlocked"); 
        canMove = true;
    }
}
