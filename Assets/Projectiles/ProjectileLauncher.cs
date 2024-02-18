using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    // Projectile to Spawn
    public GameObject projectile;

    // Where to Spawn the Projectiles
    public Transform spawnLocation;

    // Rotation of Projectile on Spawn
    public Quaternion spawnRotation;

    public DetectionZone detectionZone;
    public AudioSource spawnAudioSource;

    public float spawnTime = 0.5f;

    private float timeSinceSpawned = 0.5f;


    // Update is called once per frame
    void Update()
    {
        if(detectionZone.detectedObjs.Count > 0) {
            timeSinceSpawned += Time.deltaTime;

            if(timeSinceSpawned >= spawnTime) {
                Instantiate(projectile, spawnLocation.position, spawnRotation);
                
                if(spawnAudioSource.clip) {
                    
                    spawnAudioSource.Play();
                }

                timeSinceSpawned = 0;
            }
        } else {
            timeSinceSpawned = 0.5f;
        }
    }
}
