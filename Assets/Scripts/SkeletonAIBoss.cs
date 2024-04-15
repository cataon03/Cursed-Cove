using Pathfinding;
using UnityEngine;
using System;
using Yarn.Unity;
using UnityEditor.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.AI;
using Unity.VisualScripting;


public class SkeletonAIBoss : SkeletonAIBase, ICharacter
{
    public static event Action<float> OnBossHit;     

    /* For health regeneration */ 
    public Transform[] healthRegenZones; 
    DetectionZone detectionZone; 
    public float regenRate; 
    public float timeBetweenRegens; 
    int zoneIdx = 0; 
    float timeSinceLastRegen = 0f; 
    public float fastMoveSpeed; 
    ProjectileLauncher projectileLauncher;
    AutonomousAttack autonomousAttack; 
    PlayerBehaviorMonitor playerBehaviorMonitor;  

    private Enemy enemy;  

    /* Boss state regulation */ 
    public enum BossState { Aggressive, Chasing, Retreating, Regenerating } 
    public enum Health { Critical, Medium, Okay, Full }
    public BossState currentState; 
    public SkeletonBossHealthbar healthBar; 

    new public void Start(){
        base.Start(); 
        detectionZone = gameObject.GetComponentInChildren<DetectionZone>(); 
        enemy = gameObject.GetComponent<Enemy>();
        playerBehaviorMonitor = gameObject.GetComponent<PlayerBehaviorMonitor>(); 
        projectileLauncher = gameObject.GetComponent<ProjectileLauncher>();  
        projectileLauncher.setLaunchEnabled(true); 
        autonomousAttack = gameObject.GetComponent<AutonomousAttack>(); 
        autonomousAttack.setAttackEnabled(true); 
        healthBar = gameObject.GetComponentInChildren<SkeletonBossHealthbar>(); 
        healthBar.MaxValue = (int) enemy.Health; 
        healthBar.Value = (int) enemy.Health; 
        setTarget(GameObject.FindGameObjectWithTag("Player").transform);
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

    void HandleOnReleaseBoss(){
        projectileLauncher.enabled = true;
        autonomousAttack.enabled = true;
        UnlockMovement();  
    }

    public void updateHealthBar(float change){
        healthBar.Change((int) change); //todo
    }

    public Health GetHealth(){
        if (enemy._health <= enemy.maxHealth/3){
            return Health.Critical; 
        }
        else if (enemy._health <= enemy.maxHealth/2){
            return Health.Medium; 
        }
        else if (enemy._health == enemy.maxHealth){
            return Health.Full; 
        }
        else { 
            return Health.Okay; 
        }
    }

    override public void move() {
        switch (currentState){
            case BossState.Aggressive: 
                moveAggressive(); 
                break; 
            case BossState.Chasing: 
                moveChasing(); 
                break;
            case BossState.Retreating: 
                moveRetreating(); 
                break;
            case BossState.Regenerating: 
                moveRegenerating(); 
                break; 
        }
    }

    public void ChangeState(BossState newState){
        Debug.Log("Changing boss state to: " + newState.ToString()); 
        switch (newState){
            case BossState.Aggressive:
                changeAISpeed(moveSpeed); 
                setTarget(playerTransform);  
                projectileLauncher.setLaunchEnabled(true); 
                projectileLauncher.setLaunchFrequency(4.5f); // Need to make Low, Med, High enum 
                projectileLauncher.setLaunchType(ProjectileLauncher.LaunchType.Mixed); 
                break; 
            case BossState.Chasing:
                changeAISpeed(fastMoveSpeed); 
                setTarget(playerTransform);  
                projectileLauncher.setLaunchEnabled(true); 
                projectileLauncher.setLaunchType(ProjectileLauncher.LaunchType.Directional); 
                projectileLauncher.setLaunchFrequency(5f); 
                changeAISpeed(2); // Speed up 
                break; 
            case (BossState.Retreating): 
                changeAISpeed(fastMoveSpeed); 
                projectileLauncher.setLaunchEnabled(false); 
                zoneIdx += 1; 
                zoneIdx %= healthRegenZones.Length; 
                setTarget(healthRegenZones[zoneIdx]); 
                Debug.Log("moving to zone " + zoneIdx); 
                break; 
            case (BossState.Regenerating): 
                projectileLauncher.setLaunchEnabled(false);
                LockMovement(); 
                break; 
        }
        currentState = newState; 
    }

    void moveAggressive(){
        if (GetHealth() == Health.Critical){
            ChangeState(BossState.Retreating); 
        }
    }

    void moveChasing(){
        if (GetHealth() == Health.Critical){
            ChangeState(BossState.Retreating); 
        }

    }

    void moveRetreating(){
        float distanceToRegenZone = Vector3.Distance(transform.position, healthRegenZones[zoneIdx].position);
       
        // Check if the enemy is farther away from the player than the threshold distance
        if (distanceToRegenZone < 1){
            ChangeState(BossState.Regenerating); 
        }

    }

    void moveRegenerating(){
        if (GetHealth() == Health.Full){
            ChangeState(BossState.Aggressive); 
            UnlockMovement(); 
        } 
        // Player entered the health regeneration zone 
        else if (detectionZone.detectedObjs.Count > 0){
            // If health is okay, stop regenerating and start fighting again
            if (GetHealth() == Health.Okay){
                ChangeState(BossState.Aggressive); 
            }
            // Run to the other health regeneration zone 
            else {
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

    public void BossCharge(){
        animator.SetTrigger("chargeUp");
    }
   
}
