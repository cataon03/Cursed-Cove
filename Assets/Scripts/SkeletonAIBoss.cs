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

    ProjectileLauncher projectileLauncher;
    AutonomousAttack autonomousAttack; 
    PlayerBehaviorMonitor playerBehaviorMonitor;  

    private Enemy enemy;  

    /* Boss state regulation */ 
    public enum BossState { Base, Chasing, Retreating, Regenerating } 
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
        currentState = BossState.Base;
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
        Debug.Log("out");  
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

    void CheckState(){
       
    
    }

    override public void move() {
    
    }

    public void ChangeState(BossState newState){
        Debug.Log("Changing boss state to: " + newState.ToString()); 
        switch (newState){
            case (BossState.Base):
                setTarget(playerTransform);  
                projectileLauncher.setLaunchEnabled(true); 
                projectileLauncher.setLaunchFrequency(4.5f); // Need to make Low, Med, High enum 
                projectileLauncher.setLaunchType(ProjectileLauncher.LaunchType.Mixed); 
                break; 
            case (BossState.Chasing):
                setTarget(playerTransform);  
                projectileLauncher.setLaunchEnabled(true); 
                projectileLauncher.setLaunchType(ProjectileLauncher.LaunchType.Directional); 
                projectileLauncher.setLaunchFrequency(5f); 
                changeAISpeed(2); // Speed up 
                break; 
            case (BossState.Retreating): 
                projectileLauncher.setLaunchEnabled(false); 
                zoneIdx += 1; 
                if (zoneIdx == healthRegenZones.Length){
                    zoneIdx = 0; 
                }
                setTarget(healthRegenZones[zoneIdx]); 
                break; 
            case (BossState.Regenerating): 
                projectileLauncher.setLaunchEnabled(false);
                LockMovement(); 
                break; 
        }
        currentState = newState; 
    }

    public void moveOnState(BossState currentState){
        switch (currentState){
            case BossState.Base: 
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

    void moveAggressive(){
        if ((playerBehaviorMonitor.GetHealth() == PlayerBehaviorMonitor.Health.Medium 
        || playerBehaviorMonitor.GetHealth() == PlayerBehaviorMonitor.Health.High) 
        && (playerBehaviorMonitor.GetProximity() == PlayerBehaviorMonitor.Proximity.Far)){
            //ChangeState(BossState.Chasing); 
        }
        if (GetHealth() == Health.Critical){
            //ChangeState(BossState.Retreating);
        }
        
    }

    void moveChasing(){
        if (playerBehaviorMonitor.GetHealth() == PlayerBehaviorMonitor.Health.Medium || playerBehaviorMonitor.GetHealth() == PlayerBehaviorMonitor.Health.High){

        }
    }

    void moveRetreating(){
        float distanceToRegenZone = Vector3.Distance(transform.position, healthRegenZones[0].position);
        // Check if the enemy is farther away from the player than the threshold distance
        if (distanceToRegenZone < 1){
            ChangeState(BossState.Regenerating); 
        }

    }

    void moveRegenerating(){
        if (GetHealth() == Health.Full){
            ChangeState(BossState.Base); 
            UnlockMovement(); 
        } 
        else if (detectionZone.detectedObjs.Count > 0){
            if (GetHealth() == Health.Okay){
                ChangeState(BossState.Base); 
            }
            else {
                //ChangeState(BossState.Retreating);
            } 
        }
        else { // regenerate
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
