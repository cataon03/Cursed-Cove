using Pathfinding;
using UnityEngine;

public class AIStep : MonoBehaviour
{
    public Transform target;
    private IAstarAI ai;
    private Seeker seeker;
    private Path path;
    private int currentWaypoint = 0;

    void Start()
    {
        ai = GetComponent<IAstarAI>();
        seeker = GetComponent<Seeker>();

        if (seeker != null && target != null)
        {
            seeker.StartPath(transform.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
            MoveOneStep();
        }
    }

    void MoveOneStep()
    {
        if (path == null || currentWaypoint >= path.vectorPath.Count)
        {
            // No path to follow or reached the destination
            return;
        }

        // Move towards the next waypoint in the path
        Vector3 direction = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        transform.position += direction * ai.maxSpeed * Time.deltaTime; // Move one step towards the waypoint

        // Check if close enough to the next waypoint to consider it reached
        float distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
    
    }
}
