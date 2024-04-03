using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class BossController : MonoBehaviour, ICharacter
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
    SpriteRenderer spriteRenderer;
    public float damage = 1;
    public float knockbackForce = 20f;
    public float speed = 500f;
    public float nextWaypointDistance = 3f; 
    Vector3 currentPosition; 
    Vector3 lastPosition; 
    public Transform target; 
    public bool playerInRange; 
    Path path; 
    int currentWaypoint = 0; 
    bool reachedEndOfPath = false; 
    Seeker seeker; 

    public AttackZone attackZone;
    Rigidbody2D rb;
    Vector2 moveInput = Vector2.zero;
    DamageableCharacter damagableCharacter;
    bool isMoving = false;

    void Start(){
        attackZone = GetComponentInChildren<AttackZone>(); 
        if (attackZone == null){
            Debug.Log("Attack zone not detected on boss");
        }
        Events.OnBossEnabled += handleOnBossEnabled; 
        canMove = true; 
        isMoving = false;
        lastPosition = transform.position; 
        rb = GetComponent<Rigidbody2D>();
        damagableCharacter = GetComponent<DamageableCharacter>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        seeker = GetComponent<Seeker>();
        //InvokeRepeating("UpdatePath", 0f, 0.5f); 
    }
    /*
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
    */ 
    public void OnFreeze(bool isFrozen){
        if (isFrozen){
            LockMovement(); 
        }
        else {
            UnlockMovement(); 
        }
    }
    void Update(){
        
    }
    
    void OnDestroy(){
        Events.OnBossEnabled -= handleOnBossEnabled; 
    }

    void handleOnBossEnabled(){
        canMove = true; 
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

            bool shouldFaceRight = (target.position.x > transform.position.x);

            // Flip the sprite based on the player's position relative to the boss 
            spriteRenderer.flipX = !shouldFaceRight;
            gameObject.BroadcastMessage("IsFacingRight", shouldFaceRight);

            /*
            if (path == null){
                return; 
            } */ 
            /*
            if (currentWaypoint >= path.vectorPath.Count){
                reachedEndOfPath = true; 
                return; 
            }
            else {
                reachedEndOfPath = false; 
            } */ 
            
            /*
            Vector2 direction = ((Vector2) path.vectorPath[currentWaypoint] - rb.position).normalized; 
            Vector2 force = direction * speed * Time.deltaTime; 
            
           float directionToTarget = target.position.x - rb.position.x;

        Debug.Log(directionToTarget); 
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
                animator.SetTrigger("attack");
            }
            */ 
            if (attackZone.playerDetected){
                //Debug.Log("attack"); 
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
        canMove = false;
    }

    public void UnlockMovement() {
        canMove = true;
    }
}
