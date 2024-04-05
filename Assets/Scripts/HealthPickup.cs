
using UnityEngine;
using Yarn.Unity;

public class HealthPickup : MonoBehaviour
{
    PlayerHealthBar healthBar;
    public int healthBonus = 1;
    public DialogueRunner dialogueRunnerChicken;


    void Awake()
    {
        healthBar = FindObjectOfType<PlayerHealthBar>();
    }

     void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            dialogueRunnerChicken.StartDialogue("TraderSystem");
            if(healthBar.health < PlayerHealthBar.MAX_HEALTH){
                healthBar.health += healthBonus; 
                Destroy(gameObject);
            }
            else{
                Destroy(gameObject);
            }

        }

       // if(healthBar.health < healthBar.maxHealth)
       // {
        //    Destroy(gameObject);
        //    healthBar.health = healthBar.health + healthBonus;
       // }
    }
}


