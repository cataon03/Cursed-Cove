using UnityEngine;

public abstract class SkeletonBase : MonoBehaviour, ICharacter
{
    public bool IsMoving { 
        set {
            isMoving = value;
            if (animator){
                animator.SetBool("isMoving", isMoving);
            }
            
        }
    }
    public bool canMove; 
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public float damage = 1;
    public float knockbackForce = 20f;
    public float moveSpeed = 500f;
    public Rigidbody2D rb;
    DamageableCharacter damagableCharacter;
    bool isMoving = false;


    public void Start(){
        canMove = true; 
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

    virtual public void move(){}

    virtual public void FixedUpdate() {
        
        if (canMove) {
            move(); 
        }
        adjustGraphics();
    }

    public void OnCollisionEnter2D(Collision2D collision) {
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
    
    public abstract void adjustGraphics(); 

    public void LockMovement() {
        canMove = false;
    }

    public void UnlockMovement() {
        canMove = true;
    }
}
