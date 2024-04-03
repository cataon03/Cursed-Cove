using Pathfinding;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class HealthGenerator : MonoBehaviour
{
    public float timeForEachIncrement; // Time to regenerate one life (1 unit)
    public float healthIncrement; 
    public List<Collider2D> detectedObjs = new List<Collider2D>();
    public float maxHealth; 
    bool isRegenerating = false;
    public GameObject target; 
    public DamageableCharacter damageableCharacter; 

    void Start(){
        damageableCharacter = target.GetComponent<DamageableCharacter>(); 
        maxHealth = damageableCharacter.Health; 
    }

    // Detect when object enters range
    void OnTriggerEnter2D(Collider2D collider) {
        if(collider.gameObject == target) {
            detectedObjs.Add(collider);
        }
    }

    // Detect when object leaves range
    void OnTriggerExit2D(Collider2D collider) {
        if(collider.gameObject == target) {
            detectedObjs.Remove(collider);
        }
    }

    void Update(){
        if (!isRegenerating && detectedObjs.Count > 0){
            Debug.Log("generate"); 
            isRegenerating = true; 
            RegenerateIncrement(); 
        }
    }

    public IEnumerator RegenerateIncrement() {
        if (damageableCharacter.Health + healthIncrement <= maxHealth){
            yield return new WaitForSeconds(timeForEachIncrement); // Wait for knockback effect to apply
            isRegenerating = false; 
        }
    }
}