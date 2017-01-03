﻿using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

    public float chaseSpeed = 4.0f;
    public float patrolSpeed = 1.0f;
    public float chaseWaitTime = 5.0f;
    public float patrolWaitTime = 2.0f;
    public GameObject[] wayPoints;

    private NavMeshAgent Agent;
    private SphereCollider DetCollider; // Ei käytössä atm, ehkä voisi muokata ilmotusrangeksi kun jahtaa vihollista?
    private EnemyBehaviour behaviour;
    private int wayPoint;
    private LastPlayerSighting lastPlayerSighting;
    private float chaseTimer;
    private float patrolTimer;

    // Use this for initialization
    void Awake () {
        Agent = GetComponent<NavMeshAgent>();

        behaviour = GetComponent<EnemyBehaviour>();

        lastPlayerSighting = FindObjectOfType<LastPlayerSighting>();
        DetCollider = GetComponent<SphereCollider>();
        wayPoint = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (behaviour.personalLastSighting != lastPlayerSighting.resetPosition)
        {
            Invoke("Chase", 0.5f);
        }

        else
        {
            if (wayPoints.Length > 0)
            {
                Patrol();
            }
                
        }

    }

    void Patrol()
    {
        Agent.speed = patrolSpeed;
        Agent.stoppingDistance = 0.5f;

        if (Agent.remainingDistance < 0.5f || Agent.destination == null)
        {
            patrolTimer += Time.deltaTime;
            if (patrolTimer > patrolWaitTime)
            {
                if (wayPoint < wayPoints.Length - 1)
                {
                    wayPoint += 1;
                }
                else
                {
                    wayPoint = 0;
                }
                patrolTimer = 0f;

                Agent.destination = wayPoints[wayPoint].transform.position;
            }
        }

    }

    void Chase()
    {   
        Vector3 sightingDeltaPos = behaviour.personalLastSighting - transform.position;
        if (sightingDeltaPos.sqrMagnitude > 10f)
            Agent.destination = behaviour.personalLastSighting;
        
        Agent.stoppingDistance = 5f;
        Agent.speed = chaseSpeed;

        if (Agent.remainingDistance < Agent.stoppingDistance)
        {
            chaseTimer += Time.deltaTime;

            // If enemy losses sight of player it waits sometime and then resets player sighting positions and returns patrolling
            if (chaseTimer >= chaseWaitTime)
            {
                lastPlayerSighting.position = lastPlayerSighting.resetPosition;
                behaviour.personalLastSighting = lastPlayerSighting.resetPosition;
                chaseTimer = 0;
            }

        }
        else
            chaseTimer = 0;

    }

    float CalculatePathLength(Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();
        if (Agent.enabled)
            Agent.CalculatePath(targetPosition, path);

        Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];
        allWayPoints[0] = transform.position;
        allWayPoints[allWayPoints.Length + 1] = targetPosition;

        for (int i = 0; i < path.corners.Length; i++)
        {
            allWayPoints[i + 1] = path.corners[i];
        }

        float pathLength = 0;

        for (int i = 0; i < allWayPoints.Length - 1; i++)
        {
            pathLength += Vector3.Distance(allWayPoints[i], allWayPoints[i + 1]);
        }

        return pathLength;
    }

}
