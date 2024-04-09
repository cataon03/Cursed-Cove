using UnityEngine;

public class SmartSkeleton : SkeletonAIBase, ICharacter
{
    public DetectionZone detectionZone; 
    private bool isAgro = false; 

    new public void Start(){
        base.Start(); 
        setTarget(GameObject.FindGameObjectWithTag("Player").transform);        
        detectionZone = gameObject.GetComponentInChildren<DetectionZone>(); 
        LockMovement(); 
    }

    override public void move() {
        if (!isAgro && detectionZone.detectedObjs.Count > 0){
            isAgro = true; 
            UnlockMovement(); 
        }
    }
}
