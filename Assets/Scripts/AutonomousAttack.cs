using UnityEngine;
using System;
using Pathfinding;
using System.Data.Common;


public class AutonomousAttack : MonoBehaviour
{
    public float attackFrequency; 
    public DetectionZone detectionZone; 
    public bool attacksWithAnimation = false; 
    private float timeSinceLastAttack; 
    private bool attackEnabled = true;
    private SkeletonAIBase skeletonAIBase; 
    private DamageableCharacter player;  

    public void Start(){
        skeletonAIBase = gameObject.GetComponentInParent<SkeletonAIBase>(); 
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<DamageableCharacter>(); 
    }

    void FixedUpdate(){
        if (timeSinceLastAttack >= attackFrequency){
            if (skeletonAIBase.getCanAttack()){
                if (attackEnabled && detectionZone.detectedObjs.Count > 0){
                    if (attacksWithAnimation){
                        skeletonAIBase.attack(); 
                    }
                    else{
                        Vector2 direction = (player.transform.position - skeletonAIBase.transform.position).normalized; 
                        Vector2 knockback = direction * skeletonAIBase.knockbackForce;
                        player.OnHit(skeletonAIBase.damage, knockback); 
                    }                  
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