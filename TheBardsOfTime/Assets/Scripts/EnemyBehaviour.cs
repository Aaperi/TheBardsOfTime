using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {

    public float DetectionDist;
    public float ChaseSpeed;
    public float RoamSpeed;
    public float RoamWaitTime;
    public float ReturnRoamTime;
    public bool Chasing;
    public Transform[] WayPoints;

    private GameObject Player;
    private Rigidbody EnemyRGB;
    private SphereCollider DetCollider;
    private Transform Target;
    private bool Roaming;

	// Use this for initialization
	void Start () {
        Player = GameObject.FindGameObjectWithTag("Player");
        Target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        EnemyRGB = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Rigidbody>();
        DetCollider = GameObject.FindGameObjectWithTag("Enemy").GetComponent<SphereCollider>();
        DetCollider.radius = DetectionDist;
        
    }
	
	// Update is called once per frame
	void Update () {
        if (Chasing)
            Chase();
        else
            Patrol();
	}

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Chasing = true;
        }
        else
        {
            Chasing = false;
            Roaming = true;
        }
            
    }

    void Chase()
    {
        Vector3 targetDir = Target.position - transform.position;
        Physics.Raycast(transform.position, targetDir);
        //RaycastHit Hit;
        Debug.DrawRay(transform.position, targetDir, Color.green);
        float step = 10 * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
        Debug.DrawRay(transform.position, newDir, Color.red);
        transform.rotation = Quaternion.LookRotation(newDir);
        EnemyRGB.velocity = targetDir * ChaseSpeed;
    }

    void Patrol()
    {

    }
}
