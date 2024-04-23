using System; 
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageableCharacter : MonoBehaviour, IDamageable
{
    public static event Action<float> OnPlayerHit; 
    public static event Action<float> OnBossHit; 
    public static event Action OnPlayerDeath; 
    public float maxVelocity = 5f; 
    public bool hasItemDrops; 
    public GameObject itemDrops;
    public GameObject healthText;
    public bool disableSimulation = false;

    public bool canTurnInvincible = false;
    public float invincibilityTime = 0.25f;
    public Animator animator;
    Rigidbody2D rb;
    Collider2D physicsCollider;
    
    bool isAlive = true;
    private float invincibleTimeElapsed = 0f;
    private Canvas sceneCanvas;
 
    public float Health {
        set {
            // When health is dropped (new value less than old value), play hit animation and show damage taken as text
            if(value < _health) {
                OnHitActions(_health - value);
                animator.SetTrigger("hit");
                // Spawn damage text right above the character
                HealthText healthTextInstance = Instantiate(healthText).GetComponent<HealthText>();
                if (healthTextInstance == null){
                    Debug.Log("Warning: health text not initialized properly"); 
                }
                RectTransform textTransform = healthTextInstance.GetComponent<RectTransform>();
                textTransform.transform.position = Camera.main.WorldToScreenPoint(gameObject.transform.position);
                textTransform.SetParent(sceneCanvas.transform);
                healthTextInstance.textMesh.text = (_health - value).ToString();
            }

            _health = value;

            if(_health <= 0) {
                OnCharacterDeath(); 
            }
        }
        get {
            return _health;
        }
    }
    public float maxHealth; 
    
    virtual public void OnCharacterDeath(){
        if (hasItemDrops){
            Instantiate(itemDrops, transform.position, Quaternion.identity);
        }
        if (gameObject.tag == "Player"){
            OnPlayerDeath?.Invoke(); 
        }
        animator.SetBool("isAlive", false);
        Targetable = false; 
    }

    virtual public void OnHitActions(float damage){

    }

    public bool Targetable { get { return _targetable; }
    set {
        _targetable = value;

        if(disableSimulation) {
            rb.simulated = false;
        }

        physicsCollider.enabled = value;
    } }

    public bool Invincible { get {
        return _invincible;
     }
     set {
        _invincible = value;

        if(_invincible == true) {
            invincibleTimeElapsed = 0f;
        }
     } }

    public bool ProjectileInvincible { get {
        return _projectile_invincible;
     }
     set {
        _projectile_invincible = value;

        if(_projectile_invincible == true) {
            invincibleTimeElapsed = 0f;
        }
     } }

    public float _health = 5;
    public bool _targetable = true;

    public bool _invincible = false;
    public bool _projectile_invincible = false;

    private void OnDestroy()
    {
    }
    
    public void Start(){
        maxHealth = Health; 

        animator = GetComponent<Animator>();

        // Make sure the enemy is alive at the start of it's script
        animator.SetBool("isAlive", isAlive);

        rb = GetComponent<Rigidbody2D>();
        physicsCollider = GetComponent<Collider2D>();

        if(healthText == null) {
            Debug.LogWarning("Health text prefab is not set on " + gameObject.name);
        }
        
        sceneCanvas = GameObject.FindGameObjectWithTag("Main Canvas").GetComponent<Canvas>();

        if(sceneCanvas == null) {
            Debug.LogWarning("No canvas object found in scene by " + gameObject.name);
        }
    }

    // Take damage with knockback
    virtual public void OnHit(float damage, Vector2 knockback)
    {
        if(!Invincible) {
            Health -= damage;
            // Apply force 
            // Impulse for instantaneous forces
            rb.AddForce(knockback, ForceMode2D.Impulse);
        
            if (canTurnInvincible) {
                // Activate invincibility and timer 
                Invincible = true;
            }
        }
       
        if (gameObject.tag == "Player"){
            OnPlayerHit?.Invoke(Health); 
        }
    }

    /// Take damage without knocback
    public void OnHit(float damage)
    {
        if(!Invincible) {
            Health -= damage;

            if(canTurnInvincible) {
                // Activate invincibility and timer 
                Invincible = true;
            }
        }
        if (gameObject.tag == "Player"){
            OnPlayerHit?.Invoke(Health); 
        }
    }

    public void OnObjectDestroyed()
    {
        Destroy(gameObject);
        OnDestroy();
    }


    public void FixedUpdate() {
        if(Invincible) {
            invincibleTimeElapsed += Time.deltaTime;

            if (invincibleTimeElapsed > invincibilityTime) {
                Invincible = false;
            }
        }
    }
}
