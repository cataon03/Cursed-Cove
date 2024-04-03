
using Pathfinding;

public class Enemy : DamageableCharacter {
    override public void OnCharacterDeath(){
        // Check what type of skeleton that the enemy is
        if (gameObject.tag == "AI"){
            gameObject.GetComponent<AILerp>().canMove = false; 
            gameObject.GetComponent<AILerp>().speed = 0; 
        }
        else {
            SetPositionFreeze(true); 
        }
        Targetable = false; 
        animator.SetBool("isAlive", false);
    }
}