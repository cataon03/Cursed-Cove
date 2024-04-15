
using System; 

public class Enemy : DamageableCharacter {
    public static event Action OnBossDeath;

    new public void Start(){
        base.Start(); 
        DialogueManager.OnEnemyFreeze += HandleOnEnemyFreeze; 
    }

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

    void HandleOnEnemyFreeze(bool isFrozen){
        if (isFrozen){
            //LockMovement(); 
        }
        else {
           // UnlockMovement();
        }
    }
}