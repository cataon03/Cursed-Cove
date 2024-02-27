using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Constant speed of the projectile
    public float moveSpeed = 5f;

    // Time until projectile expires
    public float timeToLive = 5f;
    private float timeSinceSpawned = 0f;

    // Impulse force power of the arrow on collision
    public float knockbackForce = 200f;

    // Damage dealt to target on collision
    public float damage = 1f;

    // Update is called once per frame
    void Update()
    {
        transform.position += moveSpeed * transform.right * Time.deltaTime;

        timeSinceSpawned += Time.deltaTime;

        if(timeSinceSpawned > timeToLive) {
            Destroy(gameObject);
        }
    }

    // Deal damage and knockback to player then remove the projectile from the game
    void OnTriggerEnter2D(Collider2D collider) {
        string tag = collider.gameObject.tag;
        
        if(tag == "Player") {
            // Hit a player so deal damage to it
            IDamageable dCharacter = collider.gameObject.GetComponent<IDamageable>();


            if(dCharacter != null) {
                // Transform.right is the head direction of the arrow when the sprite faces right by default
                // You may need a different way to pick a knockback direction if your sprite art doesn't face right
                dCharacter.OnHit(damage, transform.right * knockbackForce);
                Destroy(gameObject);
            }
            
        }
    }
}
