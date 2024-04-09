using UnityEngine;
using System;


public class AutonomousAttack : SkeletonAIBase
{
    public string attackAnimName; 
    public float attackFrequency; 
    public float timeSinceLastAttack; 

    private DetectionZone detectionZone; 
    private bool attackEnabled = false; 

    new public void Start(){
        base.Start(); 
        detectionZone = gameObject.GetComponentInChildren<DetectionZone>(); 
    }

    new void FixedUpdate(){
        if (timeSinceLastAttack >= attackFrequency){
            if (getCanAttack()){
                if (attackEnabled && detectionZone.detectedObjs.Count > 0){
                    animator.SetTrigger(attackAnimName); 
                    timeSinceLastAttack = 0f;  
                }
            }
        }
        else {
            timeSinceLastAttack += Time.deltaTime; 
        }      
    }

    public void setAttackEnabled(bool attackEnabled){
        this.attackEnabled = attackEnabled; 
    }

    public bool getAttackEnabled(){
        return attackEnabled; 
    }
}