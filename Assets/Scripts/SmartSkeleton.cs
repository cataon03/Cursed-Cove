using UnityEngine;

public class SmartSkeleton : SkeletonAIBase, ICharacter
{
    public AttackZone attackZone; 
    private bool isAgro; 

    new public void Start(){
        base.Start(); 
        setTarget(GameObject.FindGameObjectWithTag("Player").transform);
        setCanAIMove(false); 
        
        attackZone = gameObject.GetComponentInChildren<AttackZone>(); 
    }

    override public void move() {
        if (attackZone.playerDetected){
            isAgro = true; 
            setCanAIMove(true); 
        }

        if (canMove && isAgro){            
            float distanceToPlayer = Vector2.Distance(rb.position, getTarget().position);
            
            if (distanceToPlayer <= minDistanceToPlayer) {
                IsMoving = false; 
            }
            else {
                IsMoving = true; 
            }
        }
        if (!canMove){
            setCanAIMove(false); 
            IsMoving = false; 
        }
    }
}
