using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Profiling;
using UnityEngine.AI;

public class NavMeshEnemy : MonoBehaviour
{
    [SerializeField] Transform target; 
    NavMeshAgent agent; 

    private void Start(){
        agent = GetComponent<NavMeshAgent>(); 
        agent.updateRotation = false; 
        agent.updateUpAxis = false; 

    }

    private void Update()
    {
        agent.SetDestination(target.position); 
        
    }

}