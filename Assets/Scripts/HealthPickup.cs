
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    PlayerHealthBar healthBar;
    DamageableCharacter damageableCharacter; 
    public int healthBonus = 1;

    void Awake()
    {
        damageableCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<DamageableCharacter>();
        healthBar = FindObjectOfType<PlayerHealthBar>();
    }

     void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (damageableCharacter.Health + healthBonus <= damageableCharacter.maxHealth){
                damageableCharacter.Health += healthBonus; 
            }
            else {
                damageableCharacter.Health = damageableCharacter.maxHealth; 
            }
            healthBar.UpdateHealthBar(damageableCharacter.Health); 
            Destroy(gameObject);
        }
    }
}


