using Pathfinding;
using UnityEngine;
using System;
using Yarn.Unity;
using UnityEditor.Rendering;
using UnityEngine.Rendering.Universal;


public class BossSkeleton : SkeletonAIBase, ICharacter
{
    public Transform[] healthRegenZones; 
    public static event Action<float> OnBossHit; 
    public float timeBetweenRegens; 
    float timeSinceLastHit; 
    float span; // Time span over which to determine whether player is moving away/towards enemy 
    AttackZone attackZone; 
    ProjectileLauncher projectileLauncher;
    PlayerBehaviorMonitor playerBehaviorMonitor;  
    private bool isAgro; 
    float maxHealth = 0; 
    private Enemy enemy; 
    public enum BossState { Aggressive, Chasing, Retreating, Regenerating } 
    public enum Health { Critical, Medium, Okay }
    public Bar healthBar; 
    public BossState currentState; 
    public PlayerBehaviorMonitor.PlayerBehavior playerBehavior; 
    public bool movementLocked = false; 

    new public void Start(){
        base.Start(); 
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
        else { // Increments of health in the upper range of 1/2 to 1 aren't significant 
            return Health.Okay; 
        }
    }

    // Get oppositional boss behavior for a given player behavior, considering boss health 
    BossState getBossState(PlayerBehaviorMonitor.PlayerBehavior playerState){
        Health health = GetHealth(); 

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
    }
    
    override public void FixedUpdate(){
        if (!currentlyGettingKnockback){
            checkMoveCloser(); 
            moveOnState(currentState);  
            adjustGraphics(); 
        }
    }


    override public void move() {
        playerBehavior = playerBehaviorMonitor.getPlayerBehaviorState(); 
        currentState = getBossState(playerBehavior); 
        moveOnState(currentState); 
    }

    public void moveOnState(BossState currentState){
        switch (currentState){
            case BossState.Aggressive: 
                moveAggressive(); 
                break; 
            case BossState.Chasing: 
                moveChasing(); 
                break ;
            case BossState.Retreating: 
                moveRetreating(); 
                break;
            case BossState.Regenerating: 
                moveRegenerating(); 
                break; 
        }

    }
    void moveAggressive(){
        projectileLauncher.setLaunchEnabled(true); 
        projectileLauncher.setLaunchFrequency(1f); // Need to make Low, Med, High enum 
        projectileLauncher.setLaunchType(ProjectileLauncher.LaunchType.Directional); 

        if (attackZone.playerDetected){
            //Debug.Log("attack"); 
            //animator.SetTrigger("attack");
        }
    }
    
    void moveProtective(){
        Debug.Log("Moving protectively"); 
        
        projectileLauncher.setLaunchEnabled(true); 
        projectileLauncher.setLaunchType(ProjectileLauncher.LaunchType.Mixed); 
        projectileLauncher.setLaunchFrequency(2.5f); 

        if (attackZone.playerDetected){
            Debug.Log("blocking"); 
            //animator.SetTrigger("block");
        }
    }

    void moveChasing(){
        Debug.Log("Chasing"); 
        
        projectileLauncher.setLaunchEnabled(true); 
        projectileLauncher.setLaunchType(ProjectileLauncher.LaunchType.Mixed); 
        projectileLauncher.setLaunchFrequency(2f); 
        
        changeAISpeed(2); // Speed up 
        
        if (attackZone.playerDetected){
           // Debug.Log("attack"); 
           // animator.SetTrigger("attack");
        }
    }

    void moveRetreating(){
        Debug.Log("Retreating"); 
        
        // Retreat to health regeneration zone 
        setTarget(healthRegenZones[0]); 
    }

    void moveRegenerating(){
        Debug.Log("Regenerating"); 
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
                IsMoving = false; 
                LockMovement();
            }
        }
    }
    

    
    
}
