using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding; 

public class BossController : MonoBehaviour
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
    public Transform target; 
    public float speed= 200f; 
    public float nextWaypointDistance = 3f; 
    public Transform bossGFX; 

    Path path; 
    int currentWaypoint = 0; 
    bool reachedEndOfPath = false; 
    Seeker seeker; 


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

    void OnPathComplete(Path p){
        if (!p.error){
            path = p; 
            currentWaypoint = 0; 
        } 
    }

    void FixedUpdate() {

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

            rb.AddForce(force); 

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

            if (distance < nextWaypointDistance){
                currentWaypoint++; 
            }
            if (force.x <= 0.01f){
                bossGFX.localScale = new Vector3(-1f, 1f, 1f);
            }
            else if (force.x >= -0.01f){
                bossGFX.localScale = new Vector3(1f, 1f, 1f);
            }
            Debug.Log("IsMoving"); 
            IsMoving = true;
        }
        else 
        {
            isMoving = false; 
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
