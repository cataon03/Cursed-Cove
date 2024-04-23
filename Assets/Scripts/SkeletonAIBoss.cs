using Pathfinding;
using UnityEngine;
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using System.Data.Common;


public class SkeletonAIBoss : SkeletonAIBase, ICharacter
{

    /* For health regeneration */ 
    public Transform[] healthRegenZones; 

    /* For player avoidance */ 
    public float thresholdDistance = 10f; 

    /* For boss avoidant state */ 
    public float hitCheckFreq = 10f; 
    private float timeSinceLastHitCheck = 0f; 
    private int hitsInInterval = 0; 
    public int hitThreshold = 2; 
    public DetectionZone detectionZone; 
    public float regenRate; 
    int zoneIdx = 0; 
    float timeSinceLastRegen = 0f; 
    public float fastMoveSpeed; 
    ProjectileLauncher projectileLauncher;
    AutonomousAttack autonomousAttack; 

    private Enemy enemy;  

    /* Boss state regulation */ 
    public enum BossState { Aggressive, Chasing, Avoiding, Retreating, Regenerating } 

    public enum Health { Critical, Low, Medium, High }
    public BossState currentState; 
    public SkeletonBossHealthbar healthBar; 

    new public void Start(){
        base.Start(); 

        enemy = gameObject.GetComponent<Enemy>();
        projectileLauncher = gameObject.GetComponent<ProjectileLauncher>();  
        projectileLauncher.setLaunchEnabled(true); 
        autonomousAttack = gameObject.GetComponentInChildren<AutonomousAttack>(); 
        healthBar = gameObject.GetComponentInChildren<SkeletonBossHealthbar>(); 
        healthBar.MaxValue = (int) enemy.Health; 
        healthBar.Value = (int) enemy.Health; 
        currentState = BossState.Aggressive;
        ChangeState(currentState); 
        LockMovement(); 
        autonomousAttack.enabled = false; 
        projectileLauncher.enabled = false; 
        SkeletonBossTrigger.OnReleaseBoss += HandleOnReleaseBoss; 
    }

    public void OnDestroy(){
        SkeletonBossTrigger.OnReleaseBoss -= HandleOnReleaseBoss; 
    }

    
    public void IncreaseBossHits(){
        hitsInInterval += 1; 
    }


    void HandleOnReleaseBoss(){
        projectileLauncher.enabled = true;
        autonomousAttack.enabled = true;
        UnlockMovement();  
    }

    public void updateHealthBar(float change){
        healthBar.Change((int) change); 
    }

    public Health GetHealth(){
        if (enemy._health < enemy.maxHealth/3){
            return Health.Critical; 
        }
        else if (enemy._health < enemy.maxHealth/2){
            return Health.Low; 
        }
        else if (enemy._health < enemy.maxHealth/1.5){
            return Health.Medium; 
        }
        else { 
            return Health.High; 
        }
    }

    void updateHitStats(){
       if (timeSinceLastHitCheck >= hitCheckFreq){
            timeSinceLastHitCheck = 0f;
        }
        else {
            timeSinceLastHitCheck += Time.deltaTime; 
        }      
    }

    override public void move() {
        updateHitStats(); 
        switch (currentState){
            case BossState.Aggressive: 
                moveAggressive(); 
                break; 
            case BossState.Retreating: 
                moveRetreating(); 
                break;
            case BossState.Regenerating: 
                moveRegenerating(); 
                break; 
            case BossState.Avoiding: 
                moveAvoiding(); 
                break; 
        }
    }

    public void ChangeState(BossState newState){
        Debug.Log("Changing boss state to: " + newState.ToString()); 
        switch (newState){
            case BossState.Aggressive:
                changeAISpeed(moveSpeed); 
                autonomousAttack.setAttackEnabled(true);  
                projectileLauncher.setLaunchEnabled(true); 
                projectileLauncher.setLaunchFrequency(4.5f); // Need to make Low, Med, High enum 
                projectileLauncher.setLaunchType(ProjectileLauncher.LaunchType.Mixed); 
                break; 
            case BossState.Retreating: 
                changeAISpeed(fastMoveSpeed); 
                projectileLauncher.setLaunchEnabled(false); 
                zoneIdx += 1; 
                zoneIdx %= healthRegenZones.Length; 
                setTarget(healthRegenZones[zoneIdx]); 
                Debug.Log("moving to zone " + zoneIdx); 
                break; 
            case BossState.Regenerating: 
                projectileLauncher.setLaunchEnabled(false);
                LockMovement(); 
                break; 
            case BossState.Avoiding: 
                changeAISpeed(fastMoveSpeed); 
                setDistanceTarget(thresholdDistance); 
                break; 
        }
        currentState = newState; 
    }

    void moveAggressive(){
        if (GetHealth() == Health.Critical){
            ChangeState(BossState.Retreating); 
        }
        else if (hitsInInterval > hitThreshold){
            ChangeState(BossState.Avoiding); 
        }
    }

    void moveAvoiding(){
        if (GetHealth() == Health.Critical){
            ChangeState(BossState.Retreating);
        }
        if (!IsTargetPositionWalkable()){
            setDistanceTarget(thresholdDistance); 
        }
      
    }
   
    void moveRetreating(){
        float distanceToRegenZone = Vector3.Distance(transform.position, getTarget().position);
       
        // Check if the enemy is farther away from the player than the threshold distance
        if (distanceToRegenZone < 1){
            ChangeState(BossState.Regenerating); 
        }

    }

    void moveRegenerating(){
        if (GetHealth() == Health.High){
            ChangeState(BossState.Aggressive); 
            UnlockMovement(); 
        } 
        // Player entered the health regeneration zone 
        else if (detectionZone.detectedObjs.Count > 0){
            // If health is okay, stop regenerating and start fighting again
            if (GetHealth() == Health.Medium){
                ChangeState(BossState.Avoiding); 
            }
            // Run to the other health regeneration zone 
            else {
                UnlockMovement(); 
                ChangeState(BossState.Retreating);
            } 
        }
        // Boss is safe, regen health 
        else { 
            if (timeSinceLastRegen >= regenRate){
                enemy.Health += 1;
                updateHealthBar(-1); 
                timeSinceLastRegen = 0f; 
            }
            else {
                timeSinceLastRegen += Time.deltaTime; 
            }
        }
    }

}
