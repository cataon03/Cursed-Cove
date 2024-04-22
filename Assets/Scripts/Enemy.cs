using System; 
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : DamageableCharacter {
    public static event Action OnBossDeath;
    public SkeletonBossHealthbar healthbar; 

    new public void Start(){
        base.Start(); 
        healthbar = gameObject.GetComponentInChildren<SkeletonBossHealthbar>();
        DialogueManager.OnEnemyFreeze += HandleOnEnemyFreeze; 
    }

    override public void OnCharacterDeath(){
        // Check what type of skeleton that the enemy is
        if (gameObject.tag == "Boss"){
            SkeletonAIBase aISkeletonBase = gameObject.GetComponent<SkeletonAIBase>();
            aISkeletonBase.lockAttack(); 
            aISkeletonBase.LockMovement(); 
            animator.SetBool("isAlive", false); 
            OnBossDeath?.Invoke(); 
        }
        Targetable = false; 
        animator.SetBool("isAlive", false);
    }

    public override void OnHitActions(float damage)
    {
        if (gameObject.tag == "Boss"){
            healthbar.Change((int) damage); 
        }
    }

    void HandleOnEnemyFreeze(bool isFrozen){
        if (isFrozen){
            //LockMovement(); 
        }
        else {
           // UnlockMovement();
        }
    }
}