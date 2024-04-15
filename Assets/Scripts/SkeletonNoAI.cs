using UnityEngine;

public class SkeletonNoAI : SkeletonBase
{
    private DetectionZone detectionZone;
    Vector3 lastPosition; 

    new public void Start(){
        base.Start();
        PowerupManager.OnPlayerInvisible += HandleOnPlayerInvisible;
        if (!animator){
            animator = GetComponent<Animator>(); 
        }
        detectionZone = gameObject.GetComponentInChildren<DetectionZone>(); 
        lastPosition = transform.position; 
    }

    public void OnDestroy(){
        PowerupManager.OnPlayerInvisible -= HandleOnPlayerInvisible;
    }

    public override void move()
    {
        if (detectionZone.detectedObjs.Count > 0){
            Vector2 direction = (detectionZone.detectedObjs[0].transform.position - transform.position).normalized;
            rb.AddForce(direction * moveSpeed * Time.fixedDeltaTime);  
            IsMoving = true; 
        }
        else {
            IsMoving = false; 
        }
        lastPosition = transform.position; 
    }

    public override void adjustGraphics()
    {
        if (rb.velocity.x > 0.1f)
        {
            spriteRenderer.flipX = false;
        }
        else if (rb.velocity.x < -0.1f) 
        {
            spriteRenderer.flipX = true;
        }
    }

    public void HandleOnPlayerInvisible(bool isInvisible){
        if (isInvisible){
            LockMovement(); 
            IsMoving = false; 
        }
        else {
            UnlockMovement(); 
        }
    }
}
