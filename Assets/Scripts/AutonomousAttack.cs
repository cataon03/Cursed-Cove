using UnityEngine;
using System;
using Pathfinding;


public class AutonomousAttack : MonoBehaviour
{
    public float attackFrequency; 
    public DetectionZone detectionZone; 
    private float timeSinceLastAttack; 
    private bool attackEnabled = true;
    private SkeletonAIBase skeletonAIBase;  

    public void Start(){
        skeletonAIBase = gameObject.GetComponentInParent<SkeletonAIBase>(); 
    }

    void FixedUpdate(){
        if (timeSinceLastAttack >= attackFrequency){
            if (skeletonAIBase.getCanAttack()){
                if (attackEnabled && detectionZone.detectedObjs.Count > 0){
                    skeletonAIBase.attack(); 
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