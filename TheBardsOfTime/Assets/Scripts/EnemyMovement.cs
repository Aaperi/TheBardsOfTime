using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

    public float chaseSpeed = 4.0f;
    public float patrolSpeed = 1.0f;
    public float chaseWaitTime = 5.0f;
    public float patrolWaitTime = 2.0f;
    public GameObject[] wayPoints;

    private NavMeshAgent Agent;
    private SphereCollider DetCollider;
    private EnemyBehaviour behaviour;
    private int wayPoint;
    private LastPlayerSighting lastPlayerSighting;
    private float chaseTimer;
    private float patrolTimer;

    // Use this for initialization
    void Start () {
        behaviour = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyBehaviour>();
        wayPoint = 0;
        Agent = GameObject.FindGameObjectWithTag("Enemy").GetComponent<NavMeshAgent>();
        lastPlayerSighting = FindObjectOfType<LastPlayerSighting>();
        DetCollider = GameObject.FindGameObjectWithTag("Enemy").GetComponent<SphereCollider>();
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
        
        Agent.stoppingDistance = 1f;
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
}
