using System.Collections.Generic;
using System; 
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering.Universal;

public class ProjectileLauncher : MonoBehaviour
{
    [SerializeField] public List<GameObject> projectiles = new List<GameObject>();
    //public GameObject projectile;
    //public Transform target; 
    public Transform spawnLocation;
    public Quaternion spawnRotation;
    public DetectionZone detectionZone;
    public float launchFrequency = 0.5f;
    private float timeSinceSpawned = 0.5f;
    public enum LaunchType {
        Directional, 
        Bloom, 
        Mixed
    }
    LaunchType launchType = LaunchType.Directional;
    private Transform target; 
    bool launchEnabled = false; 
    private BossSkeleton boss; 
    public SkeletonAIBase skeletonAIBase; 

    void Start(){
        target = GameObject.FindGameObjectWithTag("Player").transform; 
        boss = gameObject.GetComponentInParent<BossSkeleton>(); 
        skeletonAIBase = gameObject.GetComponentInParent<SkeletonAIBase>(); 
        launchType = LaunchType.Mixed; 
    }
    System.Random rand = new System.Random();

    void Update(){
        if (launchEnabled && skeletonAIBase.canAttack){
            //if (detectionZone.detectedObjs.Count > 0) {
            timeSinceSpawned += Time.deltaTime;

            if (timeSinceSpawned >= launchFrequency) {
                Debug.Log("launching"); 
                Launch(); 
                timeSinceSpawned = 0;
            }
        }
    }

    public void setLaunchEnabled(bool enableLaunch){
        launchEnabled = enableLaunch; 
    }

    public void setLaunchType(LaunchType type){
        launchType = type; 
    }

    public void setLaunchFrequency(float frequency){
        launchFrequency = frequency; 
    }

    public void Launch(){

        if (skeletonAIBase.canAttack){       
            boss.chargeUp();
             
            if (launchType == LaunchType.Directional){
                LaunchDirectional(); 
            }
            else if (launchType == LaunchType.Bloom){
                LaunchBloom(); 
            }
            else {
                LaunchMixed(); 
            }
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
        else if (((LaunchType) nextLaunch) == LaunchType.Bloom){
            LaunchBloom(); 
        }
    }

    // Basic single-projectile launch in the direction of the player
    public void LaunchDirectional(){
        GameObject newProjectile = Instantiate(pickProjectilePrefab(), transform.position, spawnRotation);
        Vector2 direction = (target.position - transform.position).normalized;
        newProjectile.GetComponent<Projectile>().Launch(direction);
    }

    // Multiple projectile launches moving outwards in a ring 
    public void LaunchBloom(){
        int numberOfProjectiles = 5; 
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