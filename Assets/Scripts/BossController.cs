using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class BossController : MonoBehaviour, ICharacter
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
    public string attackAnimName = "bossAttack";
    public bool canAttack; 
    Rigidbody2D rb;
    Vector2 moveInput = Vector2.zero;
    DamageableCharacter damagableCharacter;
    bool isMoving = false;
    public Transform target; 
    public float speed= 200f; 
    public float nextWaypointDistance = 3f; 
    public Transform bossGFX; 

    Path path; 
    int currentWaypoint = 0; 
    bool reachedEndOfPath = false; 
    Seeker seeker; 

    void Awake(){
        DialogueTriggerWithCollider.OnCharacterFreeze += OnFreeze;
    }

    void Start(){
        SceneContext.OnPlayerFreeze += handleOnPlayerFreeze;
        canMove = true; 
        seeker = GetComponent<Seeker>(); 
        lastPosition = transform.position; 
        rb = GetComponent<Rigidbody2D>();
        damagableCharacter = GetComponent<DamageableCharacter>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        InvokeRepeating("UpdatePath", 0f, 0.5f); 
        
    }

    void UpdatePath(){
        if (seeker.IsDone()){
            seeker.StartPath(rb.position, target.position, OnPathComplete); 
        }
    }

    public void OnFreeze(bool isFrozen){
        if (isFrozen){
            LockMovement(); 
        }
        else {
            UnlockMovement(); 
        }
    }
    void OnPathComplete(Path p){
        if (!p.error){
            path = p; 
            currentWaypoint = 0; 
        } 
    }


    
    void FixedUpdate(){
        if (canAttack && detectionZone.detectedObjs.Count> 0){
            Debug.Log("player within attacking range"); 
           animator.SetTrigger(attackAnimName);
        }
    }
    void Update() {

        if (canMove){
            
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
            if (force.x <= 0.01f){
                spriteRenderer.flipX = true;
            }
            else {
                spriteRenderer.flipX = false;
            }
            rb.AddForce(force); 

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

            if (distance < nextWaypointDistance){
                currentWaypoint++; 
            }
           
            IsMoving = true;
        }
        else 
        {
            isMoving = false; 
        }
        lastPosition = transform.position; 

        // check if boss can attack 

    }
    
    void attack() {
        if (canAttack) {
            animator.SetTrigger(attackAnimName);
        }
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

    public void handleOnPlayerFreeze(bool frozen){
        if (frozen){
            LockMovement(); 
        }
        else {
            UnlockMovement(); 
        }
    }

    public void LockMovement() {
        canMove = false;
    }

    public void UnlockMovement() {
        canMove = true;
    }
}
