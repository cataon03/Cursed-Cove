using System;
using Pathfinding;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerBehaviorMonitor : MonoBehaviour
{
    public float[] distanceThreshold = new float[2]; // Thresholds for determining proximity to boss 
    public float[] hitThreshold = new float[2];
    
    public float[] healthThreshold = new float[2]; 

    public float analysisInterval; // Time interval to check hit rate 


    public enum Proximity { Close, Medium, Far }
    public enum HitRate { Low, Medium, High } 
    public enum Health { Critical, Medium, High }
    private float hitFreq = 0f; 
    private float timeSinceLastCheck = 0f; 

    private GameObject player; 

    public void Start(){
        BossSkeleton.OnBossHit += HandleOnBossHit; 
        player = GameObject.FindGameObjectWithTag("Player"); 

    }

    public Proximity GetProximity(){
        float dist = Vector2.Distance(transform.position, player.transform.position);

        if (dist <= distanceThreshold[0])
        {
            return Proximity.Close;
        }
        else if (dist <= distanceThreshold[1])
        {
            return Proximity.Medium;
        }
        else 
        {
            return Proximity.Far;
        }
    }

    public HitRate GetHitRate(){
        if (hitFreq <= hitThreshold[0]){
            return HitRate.Low; 
        }
        else if (hitFreq <= hitThreshold[1]){
            return HitRate.Medium; 
        }
        else {
            return HitRate.High; 
        }
    }

    public Health GetHealth(){
        float health = player.GetComponent<DamageableCharacter>()._health; 
        float maxHealth = player.GetComponent<DamageableCharacter>().maxHealth; 

        if (health <= maxHealth/3){
            return Health.Critical; 
        }
        if (health <= (2 * maxHealth)/(3)){
            return Health.Medium; 
        }
        else {
            return Health.High;
        }
    }

    void HandleOnBossHit(float health){
        hitFreq += 1; 
    }

    void Update(){

        if (timeSinceLastCheck >= analysisInterval) {
            timeSinceLastCheck = 0;
            hitFreq = 0; 
        }
        else {
            timeSinceLastCheck += Time.deltaTime;
        }
    }
}
