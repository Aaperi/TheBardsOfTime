using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

    public float ChaseSpeed;
    public float RoamSpeed;
    public float RoamWaitTime;
    public float ReturnRoamTime;
    public Transform[] WayPoints;

    private Rigidbody EnemyRGB;
    private SphereCollider DetCollider;
    private Transform Target;
    private EnemyBehaviour behaviour;

    // Use this for initialization
    void Start () {
        EnemyRGB = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Rigidbody>();
        DetCollider = GameObject.FindGameObjectWithTag("Enemy").GetComponent<SphereCollider>();
        behaviour = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyBehaviour>();
        Target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        DetCollider.radius = behaviour.DetectionDist;
    }
	
	// Update is called once per frame
	void Update () {
	
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

    public void Patrol()
    {

    }
}
