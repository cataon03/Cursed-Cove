using System.Collections;
using Pathfinding;
using UnityEngine;

public abstract class SkeletonAIBase : Skeleton, ICharacter
{
    public float hitDelay; 
    public float minDistanceToPlayer = 1.2f; 
    public bool playerInRange; 
    public float speed; 
    private bool canAttack;
    private AIDestinationSetter destinationSetter;
    public AILerp aiLerp; 
    public Transform playerTransform; 

    new public void Start(){
        base.Start();
        aiLerp = gameObject.GetComponent<AILerp>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; 
        destinationSetter = GetComponent<AIDestinationSetter>();
        setTarget(GameObject.FindGameObjectWithTag("Player").transform);
        canAttack = true; 
    }


    override public void adjustGraphics(){
        if (destinationSetter){
            bool shouldFaceRight = destinationSetter.target.position.x > transform.position.x;
            spriteRenderer.flipX = !shouldFaceRight; 
            //gameObject.BroadcastMessage("IsFacingRight", shouldFaceRight);
        }
        
        if (aiLerp.enabled && aiLerp.velocity.magnitude > 0.1f){
            IsMoving = true; 
        }
        else {
            IsMoving = false; 
        }
    }

    public bool getCanAttack(){
        return canAttack; 
    }

    public void lockAttack(){
        Debug.Log("unlock attack"); 
        canAttack = false; 
    }

    public void unlockAttack(){
        Debug.Log("unlock attack"); 
        canAttack = true; 
    }

    public void setTarget(Transform newTarget){
        destinationSetter.target = newTarget; 
    }

    public Transform getTarget(){
        return destinationSetter.target; 
    }

    new public void LockMovement() {
        aiLerp.enabled = false; 
    }

    new public void UnlockMovement() {
        aiLerp.enabled = true; 
    }

    public bool isTooClose(){
        float dist = Vector2.Distance(transform.position, playerTransform.position);
        if (dist <= minDistanceToPlayer){
            return true; 
        }
        return false; 
    }

    public void Update() {
        if (isTooClose()){
            aiLerp.isStopped = true; 
        }
        else {
            aiLerp.isStopped = false; 
        }

        move(); 
        adjustGraphics();
    }

    public void changeAISpeed(float amount){
        aiLerp.speed += amount; 
    }
}
