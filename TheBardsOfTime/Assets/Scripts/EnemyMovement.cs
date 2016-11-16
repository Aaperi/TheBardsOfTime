using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

    public float chaseSpeed = 4.0f;
    public float patrolSpeed = 1.0f;
    public float chaseWaitTime = 5.0f;
    public float patrolWaitTime = 2.0f;
    public GameObject[] wayPoints;

    private Rigidbody EnemyRGB;
    private NavMeshAgent Agent;
    private SphereCollider DetCollider;
    private Transform Target;
    private EnemyBehaviour behaviour;
    private int DestPoint = 0;
    private int wayPoint;
    private LastPlayerSighting lastPlayerSighting;
    private float chaseTimer;
    private float patrolTimer;

    // Use this for initialization
    void Start () {
        EnemyRGB = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Rigidbody>();
        DetCollider = GameObject.FindGameObjectWithTag("Enemy").GetComponent<SphereCollider>();
        behaviour = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyBehaviour>();
        Target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        wayPoint = 0;
        //DetCollider.radius = behaviour.DetectionDist;
        Agent = GameObject.FindGameObjectWithTag("Enemy").GetComponent<NavMeshAgent>();
        lastPlayerSighting = FindObjectOfType<LastPlayerSighting>();
       // GotoNext();
    }
	
	// Update is called once per frame
	void Update () {

        //if (Agent.remainingDistance < 0.5f)
        //GotoNext();

        if (behaviour.personalLastSighting != lastPlayerSighting.resetPosition)
        {
            Chase();
            //Invoke("Chase", 0.5f);
        }
        else
        {
            if (wayPoints.Length > 0)
            {
                Patrol();
            }
                
        }
    }


    /* void GotoNext()
     {
         if (WayPoints.Length == 0)
             return;

         Agent.destination = WayPoints[DestPoint].position;
         DestPoint = (DestPoint + 1) % WayPoints.Length;
     }*/

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
        /* Vector3 targetDir = Target.position - transform.position;
         //Physics.Raycast(transform.position, targetDir);
         float step = 10 * Time.deltaTime;
         Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
         Debug.DrawRay(transform.position, newDir, Color.red);
         transform.rotation = Quaternion.LookRotation(newDir);
         EnemyRGB.velocity = targetDir * ChaseSpeed;*/
        
        Vector3 sightingDeltaPos = behaviour.personalLastSighting - transform.position;
        if (sightingDeltaPos.sqrMagnitude > 4f)
            Agent.destination = behaviour.personalLastSighting;

        Agent.stoppingDistance = 1;
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
