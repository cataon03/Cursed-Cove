
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    PlayerHealthBar healthBar;
    public int healthBonus = 1;

    void Awake()
    {
        healthBar = FindObjectOfType<PlayerHealthBar>();
    }

     void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(healthBar.health < PlayerHealthBar.MAX_HEALTH){
                healthBar.health += healthBonus; 
                Destroy(gameObject);
            }
            else{
                Destroy(gameObject);
            }

        }
    }
}


