using Pathfinding;
using UnityEngine;

public class PatrolSkeleton : SkeletonAIBase, ICharacter
{
    public Transform[] patrolPoints; 
    public bool isAgro; 
    public int targetPoint; 
    public float nextWaypointDistance = 3f; 
    public DetectionZone detectionZone;

    new public void Start(){
        base.Start(); 
        targetPoint = 0; 
        setTarget(patrolPoints[targetPoint].transform); // Start patrol by default 
        detectionZone = GetComponentInChildren<DetectionZone>(); 
        isAgro = false; 
    }
     
    void increaseTargetInt(){
        targetPoint++; 
        if (targetPoint >= patrolPoints.Length){
            targetPoint = 0; 
        }
    }

    void patrol(){
        if (transform.position == patrolPoints[targetPoint].position){
            increaseTargetInt();
            setTarget(patrolPoints[targetPoint].transform);
        }
    }

    override public void move() {
        // Switch to agro state if player detected
        if (detectionZone.detectedObjs.Count > 0){
            isAgro = true; 
            setTarget(GameObject.FindGameObjectWithTag("Player").transform); 
        }
        
        // Patrol if player not detected
        if (!isAgro){
            patrol(); 
        }
    }
}
