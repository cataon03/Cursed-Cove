using System.Collections;
using Pathfinding;
using UnityEngine;

public abstract class SkeletonAIBase : Skeleton, ICharacter
{
    public float hitDelay; 
    public float minDistanceToPlayer = 1.0f; 
    public bool currentlyGettingKnockback; 
    public bool playerInRange; 
    public float speed; 

    private AIDestinationSetter destinationSetter;
    public AILerp aiLerp; 
    public Transform playerTransform; 

    new public void Start(){
        base.Start(); 
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; 
        destinationSetter = GetComponent<AIDestinationSetter>();
        setTarget(GameObject.FindGameObjectWithTag("Player").transform);
        aiLerp = GetComponent<AILerp>();
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
        aiLerp.canMove = false; 
        aiLerp.speed = 0; 
    }

    public void LockAndUpdateGraphics(){
        LockMovement(); 
        Debug.Log("lock"); 
        IsMoving = false; 
    }

    public void UnlockAndUpdateGraphics(){
        IsMoving = true; 
        UnlockMovement(); 
    }

    new public void UnlockMovement() {
        Debug.Log("unlock"); 
        //setCanAIMove(true); 
        aiLerp.canMove = true; 
        aiLerp.speed = speed; 
    }

    public void Update() {
        move(); 
        adjustGraphics();
    }

    public void changeAISpeed(float amount){
        aiLerp.speed += amount; 
    }

    public IEnumerator ApplyKnockbackWithDelay(Vector2 knockback) {
        LockMovement(); 
        aiLerp.enabled = false; 
        rb.AddForce(knockback, ForceMode2D.Impulse);
        Debug.Log("Waiting"); 
        currentlyGettingKnockback = true; 
        yield return new WaitForSeconds(hitDelay); // Wait for knockback effect to apply
        currentlyGettingKnockback = false; 
        aiLerp.enabled = true; 
        Debug.Log("stopwaiting"); 
        //aiLerp.Teleport(transform.position, true); 
        UnlockMovement(); 
        aiLerp.SearchPath();
  
    }
}
