using System;
using UnityEngine;
using System.Collections;
using TMPro;

public class PowerupManager : MonoBehaviour
{
    public static PowerupManager instance;
    public static event Action<bool> OnPlayerInvisible; 
 
    public bool isPoweredUp; 
    public DamageableCharacter dc; 
    public PlayerController pc; 
    public SpriteRenderer sr; 
    Animator animator; 
    private float timeRemaining = 0f; 
    private float timePassed = 0f; 

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start() {
        InventoryManager.OnPowerupEquipped += HandleOnPowerupEquipped;
        dc = GameObject.FindGameObjectWithTag("Player").GetComponent<DamageableCharacter>(); 
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>(); 
        sr = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>(); 
        animator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>(); 
    }

    void OnDestroy(){
        InventoryManager.OnPowerupEquipped -= HandleOnPowerupEquipped; 
    }

   // Handle equipping weapons here. Probably good to start out with a prompt to ask if 
   // the player actually wants to change weapons. 
   void HandleOnPowerupEquipped(Item powerup){
    
        if (powerup.powerupType == PowerupType.Speed){
            StartCoroutine(SpeedPowerup(powerup.timeForPowerup, powerup.powerupValue)); 
        }
        if (powerup.powerupType == PowerupType.Invincibility){
            StartCoroutine(ProjectileImmunityPowerup(powerup.timeForPowerup)); 
        }
        if (powerup.powerupType == PowerupType.Invisibility){
            Debug.Log("go invis"); 
            StartCoroutine(InvisibilityPowerup(powerup.timeForPowerup)); 
        }
        InventoryManager.instance.RemoveItem(powerup); 
    }

    private IEnumerator SpeedPowerup(float waitTime, float oldValue){ 
        animator.SetBool("poweredUp", true); 
        float oldMoveSpeed = pc.moveSpeed; 
        pc.moveSpeed = oldMoveSpeed*2;  
        yield return new WaitForSeconds(waitTime);
        animator.SetBool("poweredUp", false); 
        pc.moveSpeed = oldMoveSpeed; 
    }

    private IEnumerator ProjectileImmunityPowerup(float waitTime){ 
        animator.SetBool("poweredUp", true); 
        dc.ProjectileInvincible = true; 
        yield return new WaitForSeconds(waitTime);
        animator.SetBool("poweredUp", false); 
        dc.ProjectileInvincible = false; 
    }

    private IEnumerator InvisibilityPowerup(float waitTime){ 
        animator.SetBool("isInvisible", true); 
        OnPlayerInvisible?.Invoke(true); 
        yield return new WaitForSeconds(waitTime);
        animator.SetBool("isInvisible", false); 
        OnPlayerInvisible?.Invoke(false); 
    }
}

