using System;
using Pathfinding;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerBehaviorMonitor : MonoBehaviour
{
    public float[] distanceThreshold = new float[3]; // Thresholds for determining proximity to boss 
    public float[] hitThreshold = new float[3];

    public float analysisInterval; // Time interval to consider behavior patterns 
    public float subAnalysisInterval; // Time interval to take measurements for subcalculations 
    
    public enum PlayerBehavior { Aggressive, Evasive, Shy, Retreating }
    public enum Proximity { Close, Medium, Far, Undefined }
    public enum HitRate { High, Medium, Low }

    public PlayerBehavior currentBehaviorPattern; 
    Enemy enemy; 
    Transform playerTransform; 


    // Internal parameters calculated per interval 
    float timeSinceLastCheck = 0f; 
    float timeSinceLastSubCheck = 0f; 
    float avgDist = 0; // Average dist from boss over interval 
    int numHits = 0; // Num of boss hits over interval 
    bool playerRetreating = false; // General tendency towards retreating behavior 

    int numSubChecks = 0;
    float timeSinceLastHit; 
    private float previousDistanceToBoss;
    public float retreatDistanceThreshold = 1f; 

    float bossHealth; 
    public void Start(){
        currentBehaviorPattern = PlayerBehavior.Evasive; 
        BossSkeleton.OnBossHit += HandleOnBossHit; 
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        enemy = gameObject.GetComponent<Enemy>();
        bossHealth = gameObject.GetComponent<Enemy>()._health; 
    }

    public Proximity GetProximity(float dist){
        float closeThreshold = distanceThreshold[0];
        float mediumThreshold = distanceThreshold[1];
        float highThreshold = distanceThreshold[2];

        if (dist <= closeThreshold)
        {
            return Proximity.Close;
        }
        else if (dist <= mediumThreshold)
        {
            return Proximity.Medium;
        }
        else if (dist <= highThreshold)
        {
            return Proximity.Far;
        }
        else {
            return Proximity.Undefined; // Very far 
        }
    }

    public HitRate GetHitRate(int hits){
        if (hits <= hitThreshold[0]){
            return HitRate.Low; 
        }
        else if (hits <= hitThreshold[1]){
            return HitRate.Medium; 
        }
        else {
            return HitRate.High; 
        }
    }

    public PlayerBehavior getPlayerBehaviorState(){
        return currentBehaviorPattern; 
    }

    public bool isPlayerRetreating(){
        float currentDistanceToBoss = Vector3.Distance(playerTransform.position, transform.position);
        bool isRetreating = false; 

        // Check if the player's distance from the boss has increased by the threshold
        if (currentDistanceToBoss > previousDistanceToBoss + retreatDistanceThreshold){
            isRetreating = true;
        }

        previousDistanceToBoss = currentDistanceToBoss;

        return isRetreating; 
    }

    public void updateSubParams(){
        // Update distance 
        float distance = Vector2.Distance(transform.position, playerTransform.position);
        avgDist += distance; 
    }


    public PlayerBehavior calculatePlayerBehaviorState(){
        Proximity proximity = GetProximity(avgDist); 
        HitRate hitRate = GetHitRate(numHits);

        if (proximity == Proximity.Close){
            if ((hitRate == HitRate.High) || (hitRate == HitRate.Medium)){
                return PlayerBehavior.Aggressive; 
            }
            else {
                return PlayerBehavior.Evasive; 
            }   
        }
        else if (proximity == Proximity.Medium){
            if (hitRate == HitRate.High || hitRate == HitRate.Medium){
                if (playerRetreating){
                    return PlayerBehavior.Aggressive; 
                }
                return PlayerBehavior.Evasive; 
            }
            else if (proximity == Proximity.Far) {
                
                    return PlayerBehavior.Retreating; 
                
            }
            else {
                return PlayerBehavior.Shy; 
            }
        }
        else {
            return PlayerBehavior.Retreating; 
        }
    }

    void HandleOnBossHit(float health){
        numHits += 1; 
    }

    void Update(){
      if (timeSinceLastSubCheck >= subAnalysisInterval) {
            Debug.Log("Subanalysis"); 
            updateSubParams(); 
            timeSinceLastSubCheck = 0;
            numSubChecks += 1; 
        }
        else {
            timeSinceLastSubCheck += Time.deltaTime;
        }

        if (timeSinceLastCheck >= analysisInterval) {
            Debug.Log("Checking player behavior"); 
            timeSinceLastCheck = 0;
            avgDist /= numSubChecks; 
            currentBehaviorPattern = calculatePlayerBehaviorState();            
            Debug.Log("Player behavior state is: " + currentBehaviorPattern.ToString()); 
           
            // Reset interval params 
            numSubChecks = 0; 
            avgDist = 0; 
            numHits = 0; 
        }
        else {
            timeSinceLastCheck += Time.deltaTime;
        }
    }
}
