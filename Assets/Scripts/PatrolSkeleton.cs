using Pathfinding;
using UnityEngine;

public class PatrolSkeleton : SkeletonAIBase, ICharacter
{
    public Transform[] patrolPoints; 
    public bool isAgro; 
    public int targetPoint; 
    public float nextWaypointDistance = 3f; 
    public AttackZone attackZone;

    new public void Start(){
        base.Start(); 
        targetPoint = 0; 
        setTarget(patrolPoints[targetPoint].transform); // Start patrol by default 
        setCanAIMove(true); 
        attackZone = GetComponentInChildren<AttackZone>(); 
        isAgro = false; 
    }
     
    void increaseTargetInt(){
        targetPoint++; 
        if (targetPoint >= patrolPoints.Length){
            targetPoint = 0; 
        }
    }

    void moveOnPatrol(){
        IsMoving = true; 
        if (transform.position == patrolPoints[targetPoint].position){
            increaseTargetInt();
            setTarget(patrolPoints[targetPoint].transform);
        }
    }

    void moveOnAgro(){    
        float distanceToPlayer = Vector2.Distance(rb.position, getTarget().position);
        if (distanceToPlayer <= minDistanceToPlayer) {
            IsMoving = false; 
        }
        else {
            IsMoving = true; 
        }
    }

    override public void move() {
        if (attackZone.playerDetected){
            isAgro = true; 
            setTarget(GameObject.FindGameObjectWithTag("Player").transform); 
        }

        // Patrol if player not detected
        if (canMove && !isAgro){
            moveOnPatrol(); 
            IsMoving = true; 
        }
        else if (canMove && isAgro){
            moveOnAgro(); 
            IsMoving = true; 
        }
        else {
            IsMoving = false;
            setCanAIMove(false); 
        }
    }
}
