using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector2 currentDirection; 
    public float moveSpeed = 5f;
    public float timeToLive = 5f;
    private float timeSinceSpawned = 0f;
    public float knockbackForce = 200f;
    public float damage = 1f; 
    public SpriteRenderer spriteRenderer; 

    void Start(){
        spriteRenderer = GetComponent<SpriteRenderer>(); 
    }


    void OnTriggerEnter2D(Collider2D collider) {
        string tag = collider.gameObject.tag;
        
        if(tag == "Player") {
            IDamageable dCharacter = collider.gameObject.GetComponent<IDamageable>();

            if(dCharacter != null) {
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                Vector2 knockbackDirection = rb.velocity.normalized;
                dCharacter.OnHit(damage, knockbackDirection * knockbackForce);
                Destroy(gameObject);
            }
        }
    }

    public void Launch(Vector2 direction)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        currentDirection = direction; 
        rb.velocity = direction * moveSpeed;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle ); 
    }
}
