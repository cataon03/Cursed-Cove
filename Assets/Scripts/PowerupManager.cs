using System;
using UnityEngine;
using System.Collections;

public class PowerupManager : MonoBehaviour
{
    public static PowerupManager instance;
    public static event Action<bool> OnPlayerInvisible; 
    private DamageableCharacter damageableCharacter; 
    private PlayerController playerController; 
    private Animator animator; 
    private float baseMoveSpeed; 
    private PowerupState currentState = PowerupState.NoPowerup; 

    public enum PowerupState {NoPowerup, Speed, Invisibility, ProjectileImmunity}; 
    public Powerup lastPowerup; 

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
        damageableCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<DamageableCharacter>(); 
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>(); 
        baseMoveSpeed = playerController.moveSpeed; 
        animator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>(); 
    }

    void OnDestroy(){
        InventoryManager.OnPowerupEquipped -= HandleOnPowerupEquipped; 
    }

   // Handle equipping weapons here. Probably good to start out with a prompt to ask if 
   // the player actually wants to change weapons. 
   void HandleOnPowerupEquipped(Powerup powerup){    
        if (currentState != PowerupState.NoPowerup){
            StopAllCoroutines(); 
            playerController.moveSpeed = baseMoveSpeed; 
            OnPlayerInvisible?.Invoke(false); 
            damageableCharacter.ProjectileInvincible = false; 
            animator.SetBool("isInvisible", false); 
            animator.SetBool("poweredUp", false); 
            currentState = PowerupState.NoPowerup; 
        }   

        if (powerup.powerupType == PowerupType.Speed){
            ChangeState(PowerupState.Speed,  powerup); 
        }
        if (powerup.powerupType == PowerupType.Invincibility){
            ChangeState(PowerupState.ProjectileImmunity, powerup); 
        }
        if (powerup.powerupType == PowerupType.Invisibility){ 
            ChangeState(PowerupState.Invisibility, powerup); 
        }       
    }
    
    public void ChangeState(PowerupState newState, Powerup powerup){
        currentState = newState; 
        if (newState == PowerupState.Invisibility){
            StartCoroutine(InvisibilityPowerup(powerup.timeForPowerup)); 
        }
        else if (newState == PowerupState.Speed){
            StartCoroutine(SpeedPowerup(powerup.timeForPowerup, powerup.powerupValue)); 
        }
        else if (newState == PowerupState.ProjectileImmunity){
            StartCoroutine(ProjectileImmunityPowerup(powerup.timeForPowerup)); 
        }
    }

    private IEnumerator SpeedPowerup(float waitTime, float oldValue){ 
        animator.SetBool("poweredUp", true); 
        float oldMoveSpeed = playerController.moveSpeed; 
        playerController.moveSpeed = oldMoveSpeed*2;  
        yield return new WaitForSeconds(waitTime);
        animator.SetBool("poweredUp", false); 
        playerController.moveSpeed = oldMoveSpeed; 
    }

    private IEnumerator ProjectileImmunityPowerup(float waitTime){ 
        animator.SetBool("poweredUp", true); 
        damageableCharacter.ProjectileInvincible = true; 
        yield return new WaitForSeconds(waitTime);
        animator.SetBool("poweredUp", false); 
        damageableCharacter.ProjectileInvincible = false; 
    }

    private IEnumerator InvisibilityPowerup(float waitTime){ 
        animator.SetBool("isInvisible", true); 
        OnPlayerInvisible?.Invoke(true); 
        yield return new WaitForSeconds(waitTime);
        animator.SetBool("isInvisible", false); 
        OnPlayerInvisible?.Invoke(false); 
    }
}

