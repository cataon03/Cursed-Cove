using Pathfinding;
using UnityEngine;

public abstract class SkeletonAIBase : SkeletonBase, ICharacter
{
    public float minDistanceToPlayer = 1.2f; 
    protected Transform playerTransform; 
    private bool canAttack;
    private AIDestinationSetter destinationSetter;
    private SwordHitbox swordHitbox;
    private AILerp aiLerp; 

    new public void Start(){
        base.Start();
        aiLerp = gameObject.GetComponent<AILerp>();
        swordHitbox = gameObject.GetComponentInChildren<SwordHitbox>(); 
        aiLerp.speed = moveSpeed; 
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; 
        destinationSetter = GetComponent<AIDestinationSetter>();
        setTarget(GameObject.FindGameObjectWithTag("Player").transform);
        canAttack = true; 
    }


    override public void adjustGraphics(){
        if (destinationSetter){
            bool shouldFaceRight = destinationSetter.target.position.x > transform.position.x;
            spriteRenderer.flipX = !shouldFaceRight; 
            if (swordHitbox){
                gameObject.BroadcastMessage("IsFacingRight", shouldFaceRight);
            }
           
        }
        
        if (aiLerp && aiLerp.enabled && aiLerp.velocity.magnitude > 0.1f){
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
            if (aiLerp){
                aiLerp.isStopped = true; 
            }
            
        }
        else {
            if (aiLerp){
                aiLerp.isStopped = false; 
            }
        }

        move(); 
        adjustGraphics();
    }

    public void attack(){
        animator.SetTrigger("attack"); 
    }
    
    public void launchProjectiles(){
        animator.SetTrigger("chargeUp");
    }

    public void changeAISpeed(float newSpeed){
        aiLerp.speed = newSpeed; 
    }
}
