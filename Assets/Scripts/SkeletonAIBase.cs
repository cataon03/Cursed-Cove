using System.Collections;
using Pathfinding;
using Unity.VisualScripting;
using UnityEngine;

public abstract class SkeletonAIBase : Skeleton, ICharacter
{
    public float hitDelay; 
    public float minDistanceToPlayer = 1.0f; 
    public bool currentlyGettingKnockback; 
    public bool playerInRange; 
    public float speed; 
    public bool canAttack;

    private AIDestinationSetter destinationSetter;
    public AILerp aiLerp; 
    public Transform playerTransform; 

    new public void Start(){
        base.Start(); 
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; 
        destinationSetter = GetComponent<AIDestinationSetter>();
        setTarget(GameObject.FindGameObjectWithTag("Player").transform);
        aiLerp = GetComponent<AILerp>();
        canAttack = true; 
    }

    override public void adjustGraphics(){
        if (destinationSetter){
            bool shouldFaceRight = destinationSetter.target.position.x > transform.position.x;
            spriteRenderer.flipX = !shouldFaceRight; 
            gameObject.BroadcastMessage("IsFacingRight", shouldFaceRight);
        }
    }

    public void lockAttack(){
        canAttack = false; 
    }

    public void unlockAttack(){
        canAttack = true; 
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
        aiLerp.enabled = canAIMove; 
        IsMoving = canAIMove; 
        canMove = canAIMove; 
    }

    new public void LockMovement() {
        IsMoving = false; 
        aiLerp.enabled = false; 
    }

    public void LockAndUpdateGraphics(){
        LockMovement(); 
        IsMoving = false; 
    }

    public void UnlockAndUpdateGraphics(){
        IsMoving = true; 
        UnlockMovement(); 
    }

    new public void UnlockMovement() {
        aiLerp.enabled = true; 
        IsMoving = true; 
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
