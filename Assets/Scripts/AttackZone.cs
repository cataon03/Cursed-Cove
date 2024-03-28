using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Profiling;

public class AttackZone : MonoBehaviour
{
    public Animator animator; 
    public float swordDamage = 1f;
    public float reloadTime = 5f; 
    public float RELOAD_TIME = 5f; 

    public float knockbackForce = 100;
    public Collider2D swordCollider;
    public string tagTarget = "Player";

    // When object is detected, it is added to the list of actively detected objects
    public List<Collider2D> detectedObjs = new List<Collider2D>();
    public static event Action<bool> CharacterInAttackZone; 

    bool isTriggered;

    void Update () {
        if (isTriggered == true) {reloadTime -= Time.deltaTime;}
        if (reloadTime <= 0f) {
            isTriggered = false; 
            gameObject.GetComponent<CircleCollider2D>().enabled = true; 
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if(collider.gameObject.tag == tagTarget && isTriggered == false) {
            isTriggered = true; 
            reloadTime += RELOAD_TIME; 
            gameObject.GetComponent<CircleCollider2D>().enabled = false; 

            IDamageable damagableObject = collider.GetComponent<IDamageable>();

            if(damagableObject != null) {
               
                // Calculate Direction between character and enemy
                Vector3 parentPosition = transform.parent.position;

                // Offset for collision detection changes the direction where the force comes from (close to the player)
                Vector2 direction = (collider.transform.position - parentPosition).normalized;

                // Knockback is in direction of swordCollider towards collider
                Vector2 knockback = direction * knockbackForce;

                // After making sure the collider has a script that implements IDamagable, we can run the OnHit implementation and pass our Vector2 force
                damagableObject.OnHit(swordDamage, knockback);
                animator.SetTrigger("attack"); 
            }
        }
    }

    // Detect when object leaves range
    void OnTriggerExit2D(Collider2D collider) {
        /*if(collider.gameObject.tag == tagTarget) {
            detectedObjs.Remove(collider);
        }*/
    }

    
}
