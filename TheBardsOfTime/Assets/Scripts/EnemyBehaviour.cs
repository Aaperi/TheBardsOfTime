using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {
    public float DetectionDist;
    public float ChaseDist;

    private GameObject Player;
    private Transform Target;
    private EnemyMovement movement;
    private bool Roaming;
    private bool Chasing;

    // Use this for initialization
    void Start () {
        Player = GameObject.FindGameObjectWithTag("Player");
        Target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        movement = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyMovement>();
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 targetDir = Target.position - transform.position;
        ChaseDist = Vector3.Distance(transform.position, Target.position);
        RaycastHit hit;
        Ray facingRay = new Ray(transform.position, targetDir);
        Debug.DrawRay(transform.position, targetDir, Color.green);
        if (Physics.Raycast(facingRay, out hit, DetectionDist))
        {
            if(hit.collider.tag == "Player" && Chasing)
            {
                movement.Chase();
                //Debug.Log("Näkyy");
            }
        }

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

}
