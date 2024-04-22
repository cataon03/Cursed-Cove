using System.Data.Common;
using Pathfinding;
using Unity.Profiling;
using UnityEditor.Rendering;
using UnityEngine;

public abstract class SkeletonAIBase : SkeletonBase, ICharacter
{
    public float minDistanceToPlayer = 1.2f; 
    private float timer = 0f; 
    private float updateRate = 0.2f; 
    protected Transform playerTransform; 
    private bool canAttack;
    private AIDestinationSetter destinationSetter;
    private SwordHitbox swordHitbox;
    public AILerp aiLerp; 
    public GameObject objectToFollowPrefab;
    private GameObject currentAttackerTracker = null; 
    private Vector3 lastPosition;

    new public void Start(){
        base.Start();
        lastPosition = transform.position; 
        aiLerp = gameObject.GetComponent<AILerp>();
        swordHitbox = gameObject.GetComponentInChildren<SwordHitbox>(); 
        aiLerp.speed = moveSpeed; 
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; 
        destinationSetter = GetComponent<AIDestinationSetter>();
        setTarget(GameObject.FindGameObjectWithTag("Player").transform);
        setTargetToPlayer(); 
        canAttack = true; 
    }


    public void setTargetToPlayer(){
        float surroundRadius = 1f; 
        float angle = Random.Range(0, 360) * Mathf.Deg2Rad; // Convert degrees to radians
        Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * surroundRadius;
        GameObject follower = Instantiate(objectToFollowPrefab);
        follower.transform.SetParent(playerTransform); 
        destinationSetter.target = follower.transform; 
        destinationSetter.target.position = playerTransform.position + offset;
        if (currentAttackerTracker){
            Destroy(currentAttackerTracker); 
            currentAttackerTracker = null; 
        }
        currentAttackerTracker = follower; 
    }


    public void setDistanceTarget(float thresholdDistance){
        Vector3 point; 

        while (true){
            float angle = Random.Range(0, 360) * Mathf.Deg2Rad; // Convert degrees to radians
            Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * thresholdDistance;
            point = playerTransform.position + offset; 
            
            GraphNode nearestNode = AstarPath.active.GetNearest(point, NNConstraint.Default).node;
            if (nearestNode != null && nearestNode.Walkable) {
                break; 
            }
        }
        
        GameObject follower = Instantiate(objectToFollowPrefab);
        follower.transform.position = point; // Set position before parenting
        follower.transform.SetParent(playerTransform); 
        destinationSetter.target = follower.transform;

        if (currentAttackerTracker){
            Destroy(currentAttackerTracker); 
            currentAttackerTracker = null; 
        }
        currentAttackerTracker = follower; 
    }

    public bool IsTargetPositionWalkable() {
        // Get the nearest node to the target position
        NNInfo nearestNodeInfo = AstarPath.active.GetNearest(destinationSetter.target.position);

        // Check if the nearest node is walkable
        if (nearestNodeInfo.node != null && nearestNodeInfo.node.Walkable) {
            return true;
        } 
        else {
            return false;
        }
    }


    public void setDestinationSetter(bool isEnabled){
        destinationSetter.enabled = isEnabled; 
    }


    public bool canUpdateFlipGraphics(){
        if (timer >= updateRate){
            timer = 0f; 
            return true; 
        }
        timer += Time.deltaTime; 
        return false;
    }


    override public void adjustGraphics(){
        bool shouldFaceRight = true; 

        // Moving 
        if (aiLerp && aiLerp.enabled && aiLerp.velocity.magnitude> 0.1){
            IsMoving = true; 

            if (aiLerp.velocity.x < 0.1){
                shouldFaceRight = false; 
            }
            
            if (canUpdateFlipGraphics() && Vector2.Distance(transform.position, lastPosition) > 0.1){
                spriteRenderer.flipX = !shouldFaceRight; 
                if (swordHitbox){
                    gameObject.BroadcastMessage("IsFacingRight", shouldFaceRight);
                }
                lastPosition = transform.position;
            }
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

    public void SetPath(Path path){
        aiLerp.SetPath(path); 
    }

    public Vector2 getPosition(){
        return aiLerp.position; 
    }

    public void setTarget(Transform newTarget){
        destinationSetter.target = newTarget; 

        if (currentAttackerTracker){
            Destroy(currentAttackerTracker); 
            currentAttackerTracker = null;
        }
    }

    public Transform getTarget(){
        return destinationSetter.target; 
    }

    public void SetAIDestination(Vector3 destination){
        aiLerp.destination = destination; 
    }

    new public void LockMovement() {
        Debug.Log("locking"); 
        aiLerp.enabled = false; 
    }

    new public void UnlockMovement() {
        Debug.Log("unlocking"); 
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
