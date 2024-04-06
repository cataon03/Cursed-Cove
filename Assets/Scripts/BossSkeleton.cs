using Pathfinding;
using UnityEngine;
using System;
using Yarn.Unity;
using UnityEditor.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.AI;


public class BossSkeleton : SkeletonAIBase, ICharacter
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
    public Bar healthBar; 
    public bool movementLocked = false; 


    new public void Start(){
        base.Start(); 
        detectionZone = gameObject.GetComponentInChildren<DetectionZone>(); 
        enemy = gameObject.GetComponent<Enemy>();
        playerBehaviorMonitor = gameObject.GetComponent<PlayerBehaviorMonitor>(); 
        projectileLauncher = gameObject.GetComponentInChildren<ProjectileLauncher>();  
        autonomousAttack = gameObject.GetComponent<AutonomousAttack>(); 
        autonomousAttack.setAttackEnabled(true); 

        healthBar = gameObject.GetComponentInChildren<Bar>(); 
        healthBar.MaxValue = (int) enemy.Health; 
        healthBar.Value = (int) enemy.Health; 
        setTarget(GameObject.FindGameObjectWithTag("Player").transform);

        aiLerp.canMove = true; 
        currentState = BossState.Base;
        ChangeState(currentState); 
        IsMoving = true; 

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

    
    override public void FixedUpdate(){
        if (!currentlyGettingKnockback){
            if (!(currentState == BossState.Retreating)){
                checkMoveCloser();
            } 
            CheckState(); 
            moveOnState(currentState);  
            adjustGraphics(); 
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
                IsMoving = false; 
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
            ChangeState(BossState.Chasing); 
        }
        if (GetHealth() == Health.Critical){
            ChangeState(BossState.Retreating);
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
                ChangeState(BossState.Retreating);
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

    public void chargeUp(){
        animator.SetTrigger("chargeUp");
        setCanAIMove(false);  
    }

    public void checkMoveCloser(){
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        
        // Check if the enemy is farther away from the player than the threshold distance
        if (distanceToPlayer > minDistanceToPlayer){
            if (movementLocked){
                UnlockMovement();
                IsMoving = true; 
                movementLocked = false;  
            }
        }
        else {
            // The enemy is within the threshold distance, no need to move closer
            if (!movementLocked){
                movementLocked = true; 
            }
            IsMoving = false; 
            LockMovement();
            
        }
    }    
}
