using System.Data.Common;
using Pathfinding;
using Pathfinding.Util;
using Unity.Profiling;
using UnityEditor.Rendering;
using UnityEngine;

public abstract class SkeletonAIBase : SkeletonBase, ICharacter
{ 
    private float timer = 0f; 
    private float updateRate = 0.2f; 
    public float stoppingDistance = 1.2f; 
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

    Vector3 GetRandomPointAround(Vector3 center)
    {
        // Generate a random angle between 0 and 360 degrees (in radians)
        float angle = Random.Range(0, 2 * Mathf.PI);

        // Convert polar coordinates to Cartesian coordinates
        float x = center.x + stoppingDistance * Mathf.Cos(angle);
        float y = center.y + stoppingDistance * Mathf.Sin(angle);

        // Return the calculated position
        return new Vector3(x, y, 0); // Assuming a 2D game, keep z the same
    }

    public void setTargetToPlayer(){
        GameObject follower = Instantiate(objectToFollowPrefab);
        follower.transform.position = GetRandomPointAround(playerTransform.position);
        follower.transform.SetParent(playerTransform); 

        destinationSetter.target = follower.transform; 
        if (currentAttackerTracker){
            Destroy(currentAttackerTracker); 
            currentAttackerTracker = null; 
        }
        currentAttackerTracker = follower; 
    }


    public void setDistanceTarget(float thresholdDistance){
        Vector3 point; 
        Debug.Log("starting"); 
        while (true){
            LockMovement(); 
            float angle = Random.Range(0, 2 * Mathf.PI);

            float x = playerTransform.position.x + thresholdDistance * Mathf.Cos(angle);
            float y = playerTransform.position.y + thresholdDistance * Mathf.Sin(angle);

            point = new Vector3(x, y, 0); 
            GraphNode nearestNode = AstarPath.active.GetNearest(point, NNConstraint.Default).node;
            if (nearestNode != null && nearestNode.Walkable && Vector3.Distance(point, transform.position) >= thresholdDistance) {
                break; 
            }
        }
        GameObject follower = Instantiate(objectToFollowPrefab);
        follower.transform.position = point;
        follower.transform.SetParent(playerTransform); 
        destinationSetter.target = follower.transform; 
        if (currentAttackerTracker){
            Destroy(currentAttackerTracker); 
            currentAttackerTracker = null; 
        }
        currentAttackerTracker = follower;
        UnlockMovement(); 
        Debug.Log("done"); 
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

    new public void LockMovement() {
        aiLerp.enabled = false; 
    }

    new public void UnlockMovement() {
        aiLerp.enabled = true; 
    }

    public void Update() {
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
