using Pathfinding;
using UnityEngine;
using System;
using Yarn.Unity;
using UnityEditor.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.AI;


public class BossSkeleton : SkeletonAIBase, ICharacter
{
    public Transform[] healthRegenZones; 
    public float regenRate; 
    public static event Action<float> OnBossHit; 
    public float timeBetweenRegens; 
    float timeSinceLastHit; 
    float span; // Time span over which to determine whether player is moving away/towards enemy 
    AttackZone attackZone; 
    DetectionZone detectionZone; 
    ProjectileLauncher projectileLauncher;
    PlayerBehaviorMonitor playerBehaviorMonitor;  
    private bool isAgro; 
    float maxHealth = 0; 
    private Enemy enemy; 
    public enum BossState { Aggressive, Chasing, Retreating, Regenerating } 
    public enum Health { Critical, Medium, Okay, Full }
    public Bar healthBar; 
    public BossState currentState; 
    public PlayerBehaviorMonitor.PlayerBehavior playerBehavior; 
    public bool movementLocked = false; 
    int zoneIdx = 0; 
    float timeSinceLastRegen = 0f; 
    new public void Start(){
        base.Start(); 
        detectionZone = gameObject.GetComponentInChildren<DetectionZone>(); 
        enemy = gameObject.GetComponent<Enemy>();
        playerBehaviorMonitor = gameObject.GetComponent<PlayerBehaviorMonitor>(); 
        projectileLauncher = gameObject.GetComponentInChildren<ProjectileLauncher>();  
        healthBar = gameObject.GetComponentInChildren<Bar>(); 
        healthBar.MaxValue = (int) enemy.Health; 
        healthBar.Value = (int) enemy.Health; 
        setTarget(GameObject.FindGameObjectWithTag("Player").transform);
        attackZone = gameObject.GetComponentInChildren<AttackZone>(); 
        maxHealth = enemy._health; 
        aiLerp.canMove = true; 
        currentState = BossState.Retreating;
        ChangeState(currentState); 
        IsMoving = true; 
        
    }

    public void updateHealthBar(float change){
        healthBar.Change((int) change); //todo
    }

    public Health GetHealth(){
        if (!enemy){
            Debug.Log("problem"); 
        }
        float health = enemy._health; 

        if (health <= maxHealth/3){
            return Health.Critical; 
        }
        else if (health <= maxHealth/2){
            return Health.Medium; 
        }
        else if (health == maxHealth){
            return Health.Full; 
        }
        else { // Increments of health in the upper range of 1/2 to 1 aren't significant 
            return Health.Okay; 
        }
    }

    // Get oppositional boss behavior for a given player behavior, considering boss health 
    void CheckBossState(){
        Health health = GetHealth(); 
        
        /*BossState newBossState = BossState.Retreating; 

        if (newBossState != currentState){
            ChangeState(newBossState); 
        }  */ 
        /*
        switch (health){
            case (Health.Critical):
                return BossState.Retreating; 
            case (Health.Medium): 
                if ((playerState == PlayerBehaviorMonitor.PlayerBehavior.Aggressive) 
                || (playerState == PlayerBehaviorMonitor.PlayerBehavior.Evasive)) {
                    return BossState.Aggressive; 
                }
                else {
                    return BossState.Chasing; 
                }
            case (Health.Okay): 
                if ((playerState == PlayerBehaviorMonitor.PlayerBehavior.Retreating)){
                    return BossState.Chasing; 
                }
                else {
                    return BossState.Aggressive; 
                }
            default: 
                return BossState.Aggressive; 
            }

                */ 
        
    }
    
    override public void FixedUpdate(){
        if (!currentlyGettingKnockback){
            CheckBossState(); 
            if (!(currentState == BossState.Retreating)){
                checkMoveCloser();
            } 
            moveOnState(currentState);  
            adjustGraphics(); 
        }
    }


    override public void move() {
       
    }

    public void ChangeState(BossState newState){
        Debug.Log("Changing state to: " + newState.ToString()); 
        switch (newState){
            case (BossState.Aggressive):
                setTarget(playerTransform);  
                projectileLauncher.setLaunchEnabled(true); 
                projectileLauncher.setLaunchFrequency(3f); // Need to make Low, Med, High enum 
                projectileLauncher.setLaunchType(ProjectileLauncher.LaunchType.Directional); 
                break; 
            case (BossState.Chasing):
                setTarget(playerTransform);  
                projectileLauncher.setLaunchEnabled(true); 
                projectileLauncher.setLaunchType(ProjectileLauncher.LaunchType.Mixed); 
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
    void moveAggressive(){
        if (attackZone.playerDetected){
            Debug.Log("attack"); 
            animator.SetTrigger("attack");
        }
    }

    void moveChasing(){
        if (attackZone.playerDetected){
            Debug.Log("attack"); 
            animator.SetTrigger("attack");
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
            ChangeState(BossState.Aggressive); 
            UnlockMovement(); 
            IsMoving = true; 
        } 
        else if (detectionZone.detectedObjs.Count > 0){
            ChangeState(BossState.Retreating); 
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
        //enemy._targetable = false; 
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
