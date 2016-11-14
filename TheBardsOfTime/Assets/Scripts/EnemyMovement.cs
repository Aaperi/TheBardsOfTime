using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

    public float ChaseSpeed;
    public float RoamSpeed;
    public float RoamWaitTime;
    public float ReturnRoamTime;
    public float ChaseDist;
    public Transform[] WayPoints;

    private Rigidbody EnemyRGB;
    private NavMeshAgent Agent;
    private SphereCollider DetCollider;
    private Transform Target;
    private EnemyBehaviour behaviour;
    private int DestPoint = 0;

    // Use this for initialization
    void Start () {
        EnemyRGB = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Rigidbody>();
        DetCollider = GameObject.FindGameObjectWithTag("Enemy").GetComponent<SphereCollider>();
        behaviour = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyBehaviour>();
        Target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        DetCollider.radius = behaviour.DetectionDist;
        Agent = GameObject.FindGameObjectWithTag("Enemy").GetComponent<NavMeshAgent>();
        GotoNext();
    }
	
	// Update is called once per frame
	void Update () {
        ChaseDist = Vector3.Distance(transform.position, Target.position);

        if (Agent.remainingDistance < 0.5f)
            GotoNext();
    }

    public void Chase()
    {
        Vector3 targetDir = Target.position - transform.position;
        //Physics.Raycast(transform.position, targetDir);
        float step = 10 * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
        Debug.DrawRay(transform.position, newDir, Color.red);
        transform.rotation = Quaternion.LookRotation(newDir);
        EnemyRGB.velocity = targetDir * ChaseSpeed;

        
    }

    void GotoNext()
    {
        if (WayPoints.Length == 0)
            return;

        Agent.destination = WayPoints[DestPoint].position;
        DestPoint = (DestPoint + 1) % WayPoints.Length;
    }
}
