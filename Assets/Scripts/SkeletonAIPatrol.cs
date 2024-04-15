using Pathfinding;
using UnityEngine;

public class SkeletonAIPatrol : SkeletonAIBase, ICharacter
{
    public Transform[] patrolPoints; 
    public bool isAgro; 
    public int targetPoint; 
    public float nextWaypointDistance = 3f; 
    private bool blindToPlayer = false; 
    public DetectionZone detectionZone;

    new public void Start(){
        PowerupManager.OnPlayerInvisible += HandleOnPlayerInvisible;
        base.Start(); 
        targetPoint = 0; 
        setTarget(patrolPoints[targetPoint].transform); // Start patrol by default 
        detectionZone = GetComponentInChildren<DetectionZone>(); 
        isAgro = false; 
    }
     
    public void OnDestroy(){
        PowerupManager.OnPlayerInvisible -= HandleOnPlayerInvisible;
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
        if (!blindToPlayer && detectionZone.detectedObjs.Count > 0){
            isAgro = true; 
            setTarget(GameObject.FindGameObjectWithTag("Player").transform); 
        }
        
        // Patrol if player not detected
        if (!isAgro){
            patrol(); 
        }
    }

    public void HandleOnPlayerInvisible(bool isInvisible){
        if (isInvisible){
            blindToPlayer = true; 
            // Resume patrol if previously agrod on player
            if (isAgro){
                isAgro = false;
                setTarget(patrolPoints[targetPoint].transform);
            } 
        }
        else {
            blindToPlayer = false; 
        }
    }
}
