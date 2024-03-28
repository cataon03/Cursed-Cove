using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    public float swordDamage = 1f;
    public float knockbackForce = 15f;
    public Collider2D swordCollider;
    public Vector3 faceRight = new Vector3(1, -0.9f, 0);
    public Vector3 faceLeft = new Vector3(-1, -0.9f, 0);

    void Start() {
        if(swordCollider == null){
            Debug.LogWarning("Sword Collider not set");
        }
    }
   
    void OnTriggerEnter2D(Collider2D collider) {

        IDamageable damagableObject = collider.GetComponent<IDamageable>();

        if(damagableObject != null) {
            print("damageable"); 
            // Calculate Direction between character and enemy
            Vector3 parentPosition = transform.parent.position;

            // Offset for collision detection changes the direction where the force comes from (close to the player)
            Vector2 direction = (collider.transform.position - parentPosition).normalized;

            // Knockback is in direction of swordCollider towards collider
            Vector2 knockback = direction * knockbackForce;

            // After making sure the collider has a script that implements IDamagable, we can run the OnHit implementation and pass our Vector2 force
            damagableObject.OnHit(swordDamage, knockback);
        }
    }

    // Keep collider offset to 0 so that a flip to the left and a flip to the right have the same distance from the transform
    void IsFacingRight(bool isFacingRight) {
        if(isFacingRight) {
            gameObject.transform.localPosition = faceRight;
        } else {
            gameObject.transform.localPosition = faceLeft;
        }
    }

}
