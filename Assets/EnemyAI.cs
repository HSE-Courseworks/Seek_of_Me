// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class EnemyAI : MonoBehaviour {
	// What to chase
	public Transform target;

	// How many times each second we will update our path
	public float updateRate = 2f;

	// Cashing
	private Seeker seeker;
	private Rigidbody2D rb;

	// The calculated path
	public Path path;

	// The AI's speed per second
	public float speed = 300f;
    public ForceMode2D fMode;

    [HideInInspector]
	public bool pathIsEnded = false;

	// The max distance from the AI to a waypoint for it to continue to the next waypoint
	public float nextWaypointDistance = 3;

	// The waypoint we are currently moving towards
	private int currentWaypoint = 0;
	
	void Start()
    {
		seeker = GetComponent<Seeker>();
		rb = GetComponent<Rigidbody2D>();

		if (target == null)
        {
			Debug.LogError("No player found? PANIC!");
			return;
        }

		// Start a new path to the target position and return the result to the OnPathComplete method
		seeker.StartPath(transform.position, target.position, OnPathComplete);
    }

	public void OnPathComplete(Path p)
    {
		Debug.Log("We got a path. Did it have an error?" + p.error);
		if (!p.error)
        {
			path = p;
			currentWaypoint = 0;
        }
    }
}
