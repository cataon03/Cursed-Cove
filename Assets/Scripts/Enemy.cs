
using Pathfinding;
using System; 
using UnityEngine; 

public class Enemy : DamageableCharacter {
    public static event Action OnBossDeath;

    override public void OnCharacterDeath(){
        // Check what type of skeleton that the enemy is
        if (gameObject.tag == "AI"){
            gameObject.GetComponent<AILerp>().canMove = false; 
            gameObject.GetComponent<AILerp>().speed = 0; 
        }
        if (gameObject.tag == "Boss"){
            Debug.Log("boss is dead"); 
            SkeletonAIBase aISkeletonBase = gameObject.GetComponent<SkeletonAIBase>();
            aISkeletonBase.lockAttack(); 
            aISkeletonBase.LockMovement(); 
            animator.SetBool("isAlive", false); 
            OnBossDeath?.Invoke(); 
        }
        else {
            SetPositionFreeze(true); 
        }
        Targetable = false; 
        animator.SetBool("isAlive", false);
    }
}