
using System; 

public class Enemy : DamageableCharacter {
    public static event Action OnBossDeath;

    override public void OnCharacterDeath(){
        // Check what type of skeleton that the enemy is
        if (gameObject.tag == "Boss"){
            SkeletonAIBase aISkeletonBase = gameObject.GetComponent<SkeletonAIBase>();
            aISkeletonBase.lockAttack(); 
            aISkeletonBase.LockMovement(); 
            animator.SetBool("isAlive", false); 
            OnBossDeath?.Invoke(); 
        }
        Targetable = false; 
        animator.SetBool("isAlive", false);
    }
}