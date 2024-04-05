using UnityEngine;
using System;


public class AutonomousAttack : MonoBehaviour
{
    public float attackFrequency; 
    public float timeSinceLastAttack; 
    private DetectionZone detectionZone; 
    public String attackAnimName; 
    SkeletonAIBase skeletonAIBase; 
    public bool attackEnabled = false; 
    Animator animator; 

    void Start(){
        detectionZone = gameObject.GetComponentInChildren<DetectionZone>(); 
        animator = gameObject.GetComponent<Animator>(); 
        skeletonAIBase = gameObject.GetComponent<SkeletonAIBase>(); 
    }

    void Update(){
        if (skeletonAIBase.canAttack){
            timeSinceLastAttack += Time.deltaTime;
            if (timeSinceLastAttack >= attackFrequency && detectionZone.detectedObjs.Count > 0) {
                Debug.Log("Attacking"); 
                animator.SetTrigger(attackAnimName);
                timeSinceLastAttack = 0;
            }
        }
    }

    public void setAttackEnabled(bool attackEnabled){
        this.attackEnabled = attackEnabled; 
    }

    public bool getAttackEnabled(){
        return attackEnabled; 
    }
}