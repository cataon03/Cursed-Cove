using System.Collections.Generic;
using System; 
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering.Universal;

public class ProjectileLauncher : SkeletonAIBase
{
    [SerializeField] public List<GameObject> projectiles = new List<GameObject>();
    System.Random rand = new System.Random();
    public Quaternion spawnRotation;
    public float launchFrequency = 0.5f;
    private float timeSinceLastLaunch = 0f;
    public string launchAnimName;
    bool launchEnabled = false; 
    public enum LaunchType {
        Directional, 
        Bloom, 
        Mixed
    }
    LaunchType currentLaunchType = LaunchType.Directional;
    

    new public void Start(){
        base.Start(); 
        currentLaunchType = LaunchType.Mixed; 
    }

    new void FixedUpdate(){
        if (timeSinceLastLaunch >= launchFrequency){
            if (getCanAttack() && launchEnabled){  
                animator.SetTrigger(launchAnimName); 
                timeSinceLastLaunch = 0f;  
            }
        }
        else {
            timeSinceLastLaunch += Time.deltaTime; 
        }     
    }

    public void setLaunchEnabled(bool enableLaunch){
        launchEnabled = enableLaunch; 
    }

    public void setLaunchType(LaunchType type){
        currentLaunchType = type; 
    }

    public void setLaunchFrequency(float frequency){
        launchFrequency = frequency; 
    }

    public void Launch(){  
        if (launchEnabled){
            if (currentLaunchType == LaunchType.Directional){
                LaunchDirectional(); 
            }
            else if (currentLaunchType == LaunchType.Bloom){
                LaunchBloom(); 
            }
            else {
                LaunchMixed(); 
            }
            timeSinceLastLaunch = 0f;
        }
    }

    // Pick projectile prefab at random
    public GameObject pickProjectilePrefab(){
        int choice = rand.Next(0, projectiles.Count); 
        return projectiles[choice]; 
    }

    // Switch between different types of launches, pick the launch type arbitrarily 
    public void LaunchMixed(){
        int nextLaunch = rand.Next(0, Enum.GetValues(typeof(LaunchType)).Length);
        if (((LaunchType) nextLaunch) == LaunchType.Directional){
            LaunchDirectional(); 
        }
        else {
            LaunchBloom(); 
        }
    }

    // Basic single-projectile launch in the direction of the player
    public void LaunchDirectional(){
        GameObject newProjectile = Instantiate(pickProjectilePrefab(), transform.position, spawnRotation);
        Vector2 direction = (getTarget().position - transform.position).normalized;
        newProjectile.GetComponent<Projectile>().Launch(direction);
    }

    // Multiple projectile launches moving outwards in a ring 
    public void LaunchBloom(){
        int numberOfProjectiles = 8; 
        float angleStep = 360f / numberOfProjectiles;
        float angle = 0f;
        GameObject prefab = pickProjectilePrefab(); 

        for (int i = 0; i < numberOfProjectiles; i++)
        {
            Vector2 direction = new Vector2(Mathf.Sin((angle * Mathf.PI) / 180), Mathf.Cos((angle * Mathf.PI) / 180));
            GameObject proj = Instantiate(prefab, transform.position, Quaternion.identity);
            proj.GetComponent<Projectile>().Launch(direction);
            angle += angleStep;
        }
    }
}