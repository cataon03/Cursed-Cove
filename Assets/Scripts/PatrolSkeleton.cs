using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.AI;

public class PatrolSkeleton : MonoBehaviour, ICharacter
{
    bool IsMoving { 
        set {
            isMoving = value;
            animator.SetBool("isMoving", isMoving);
        }
    }

   public Transform[] patrolPoints; 
   public bool isAgro; 
    public int targetPoint; 

   public float minDistanceToPlayer = 2.6f; 
    public bool canMove; 
    Animator animator;
    SpriteRenderer spriteRenderer;
    public float damage = 1;
    public float knockbackForce = 20f;
    public float speed = 500f;
    public float nextWaypointDistance = 3f; 
    private AIDestinationSetter destinationSetter;
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
        destinationSetter = GetComponent<AIDestinationSetter>();
        targetPoint = 0; 
        destinationSetter.target = patrolPoints[targetPoint].transform;

        attackZone = GetComponentInChildren<AttackZone>(); 
        if (attackZone == null){
            Debug.Log("Attack zone not detected on patrol skeleton");
        }
        canMove = true; 
        isMoving = false;
        lastPosition = transform.position; 
        rb = GetComponent<Rigidbody2D>();
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
    void increaseTargetInt(){
        targetPoint++; 
        if (targetPoint >= patrolPoints.Length){
            targetPoint = 0; 
        }
    }

/*
    void Update()
    {
        if (transform.position == patrolPoints[targetPoint].position){
            increaseTargetInt();
            aiLerp.destination = patrolPoints[targetPoint].position;
             
        }
        //transform.position = Vector3.MoveTowards(transform.position, patrolPoints[targetPoint].position, speed * Time.deltaTime); 
    }
    */ 
    
    void OnDestroy(){
        Events.OnBossEnabled -= handleOnBossEnabled; 
    }

    void handleOnBossEnabled(){
        canMove = true; 
    }

   /* void Update(){
        if (canMove && !isAgro){
            if (transform.position == patrolPoints[targetPoint].position){
                increaseTargetInt();
                destinationSetter.target = patrolPoints[targetPoint].transform;
            }
        }
    }
*/ 

/*
    void Update(){
        if (attackZone.playerDetected){
            isAgro = true; 
        }
        else {
            isAgro = false; 
        }
    }
*/ 

    void moveOnPatrol(){
        IsMoving = true; 
        if (transform.position == patrolPoints[targetPoint].position){
            increaseTargetInt();
            destinationSetter.target = patrolPoints[targetPoint].transform;
        }
        
        bool shouldFaceRight = patrolPoints[targetPoint].position.x > transform.position.x;

        spriteRenderer.flipX = !shouldFaceRight;
        gameObject.BroadcastMessage("IsFacingRight", shouldFaceRight);
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
        // If the enemy detects the player, agro on it 
        if (isAgro){
            destinationSetter.target = target; 
        }
        else {
            destinationSetter.target = patrolPoints[targetPoint]; 
        }

        // Patrol if player not detected
        if (canMove && !isAgro){
            moveOnPatrol(); 
        }
        else if (canMove && isAgro){
            moveOnAgro(); 
        }
        else {
            IsMoving = false;
        }
   

/*
        bool shouldFaceRight = (target.position.x > transform.position.x);
        spriteRenderer.flipX = !shouldFaceRight;
        gameObject.BroadcastMessage("IsFacingRight", shouldFaceRight);

        /*
        if (attackZone.playerDetected){
            isAgro = true; 
            /*
            Debug.Log("attack"); 
            animator.SetTrigger("attack");
            
        }
        */ 
        
        //lastPosition = transform.position; 
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
