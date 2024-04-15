using System.Collections.Generic;
using System; 
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering.Universal;

public class AutonomousLaunch : MonoBehaviour
{
    public GameObject projectile; 
    public Animator animator; 
    public LaunchType launchType;
    public Quaternion spawnRotation;
    public float launchFrequency = 0.5f;
    private float timeSinceLastLaunch = 0f;
    public Transform targetTransform; // What to shoot at
    private System.Random rand = new System.Random();

    public enum LaunchType {
        Directional, 
        Bloom, 
        Mixed
    }
    
    public void Start(){
        animator = gameObject.GetComponent<Animator>(); 
    }

    public void Launch(){  
        if (launchType == LaunchType.Directional){
            LaunchDirectional(); 
        }
        else if (launchType == LaunchType.Bloom){
            LaunchBloom(); 
        }
        else {
            LaunchMixed(); 
        }
        timeSinceLastLaunch = 0f;
    }
    

    void FixedUpdate(){
        if (timeSinceLastLaunch >= launchFrequency){
            animator.SetTrigger("launch");  
            timeSinceLastLaunch = 0f;  
        }
        else {
            timeSinceLastLaunch += Time.deltaTime; 
        }     
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
        GameObject newProjectile = Instantiate(projectile, transform.position, spawnRotation);
        Vector2 direction = (targetTransform.position - transform.position).normalized;
        newProjectile.GetComponent<Projectile>().Launch(direction);
    }

    // Multiple projectile launches moving outwards in a ring 
    public void LaunchBloom(){
        int numberOfProjectiles = 5; 
        float angleStep = 360f / numberOfProjectiles;
        float angle = 0f;

        for (int i = 0; i < numberOfProjectiles; i++)
        {
            Vector2 direction = new Vector2(Mathf.Sin((angle * Mathf.PI) / 180), Mathf.Cos((angle * Mathf.PI) / 180));
            GameObject proj = Instantiate(projectile, transform.position, Quaternion.identity);
            proj.GetComponent<Projectile>().Launch(direction);
            angle += angleStep;
        }
    }
}