using System.Collections;
using Pathfinding;
using UnityEngine;

public abstract class SkeletonAIBase : Skeleton, ICharacter
{
    public float hitDelay; 
    public float minDistanceToPlayer = 1.0f; 
    public bool playerInRange; 
    public float speed; 

    private AIDestinationSetter destinationSetter;
    private AILerp aiLerp; 
    private Transform playerTransform; 

    new public void Start(){
        base.Start(); 
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; 
        destinationSetter = GetComponent<AIDestinationSetter>();
        setTarget(GameObject.FindGameObjectWithTag("Player").transform);
        aiLerp = GetComponent<AILerp>();
        aiLerp.canMove = false; 
    }

    override public void adjustGraphics(){
        if (destinationSetter){
        bool shouldFaceRight = destinationSetter.target.position.x > transform.position.x;
        spriteRenderer.flipX = !shouldFaceRight; 
        }
    }

    public void setTarget(Transform newTarget){
        destinationSetter.target = newTarget; 
    }

    public Transform getTarget(){
        return destinationSetter.target; 
    }

    public bool getCanAIMove(){
        return aiLerp.canMove; 
    }

    public void setCanAIMove(bool canAIMove){
        if (canAIMove){
            aiLerp.speed = speed; 
        }
        else {
            aiLerp.speed = 0; 
        }
        aiLerp.canMove = canAIMove; 
        IsMoving = canAIMove; 
        canMove = canAIMove; 
    }

    new public void LockMovement() {
        setCanAIMove(false); 
    }

    new public void UnlockMovement() {
        setCanAIMove(true); 
    }

    override public void FixedUpdate() {
        if (canMove && shouldMoveCloser()) {
            if (!getCanAIMove()){
                setCanAIMove(true); 
            }
            move(); 
        }
        else {
            setCanAIMove(false); 
        }
        adjustGraphics();
    }

    public bool shouldMoveCloser(){
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        // Check if the enemy is farther away from the player than the threshold distance
        if(distanceToPlayer > minDistanceToPlayer){
            // The enemy is far enough to move closer
            return true;
        }
        else {
            // The enemy is within the threshold distance, no need to move closer
            return false;
        }
    }

    public void changeAISpeed(float amount){
        aiLerp.speed += amount; 
    }

    public IEnumerator ApplyKnockbackWithDelay(Vector2 knockback) {
        aiLerp.canMove = false;
        canMove = false; 
        rb.AddForce(knockback, ForceMode2D.Impulse);

        yield return new WaitForSeconds(hitDelay); // Wait for knockback effect to apply
        
        aiLerp.Teleport(transform.position, true); 
        aiLerp.canMove = true;
        aiLerp.SearchPath();
        canMove = true; 
    }
}
