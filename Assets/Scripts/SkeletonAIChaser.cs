using UnityEngine;
using UnityEngine.AI;

public class SkeletonAIChaser : SkeletonAIBase, ICharacter
{
    public bool isNearSighted = false; 
    public DetectionZone detectionZone; 
    private bool isAgro = false; 

    new public void Start(){
        PowerupManager.OnPlayerInvisible += HandleOnPlayerInvisible;
        base.Start(); 
        setTarget(GameObject.FindGameObjectWithTag("Player").transform);        
        detectionZone = gameObject.GetComponentInChildren<DetectionZone>(); 
        LockMovement(); 
    }

    public void OnDestroy(){
        PowerupManager.OnPlayerInvisible -= HandleOnPlayerInvisible;
    }

    override public void move() {
        // Re-lock movement for proximity-based ("near-sighted") attack
        if (isNearSighted && isAgro && detectionZone.detectedObjs.Count == 0){
            isAgro = false; 
            LockMovement(); 
        }
        if (!isAgro && detectionZone.detectedObjs.Count > 0){
            isAgro = true; 
            UnlockMovement(); 
        }
    }

    public void HandleOnPlayerInvisible(bool isInvisible){
        // Freeze
        if (isInvisible){
            LockMovement(); 
            IsMoving = false; 
        }
        else {
            UnlockMovement(); 
        }
    }
}
