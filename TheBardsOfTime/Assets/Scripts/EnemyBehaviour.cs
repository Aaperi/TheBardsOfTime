using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {
    public float DetectionDist;

    private Transform Target;
    private EnemyMovement movement;
    private bool Chasing;

    // Use this for initialization
    void Start () {
        Target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        movement = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyMovement>();
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 targetDir = Target.position - transform.position;
        RaycastHit hit;
        Ray facingRay = new Ray(transform.position, targetDir); // ray vihollisesta pelaajaan
        Debug.DrawRay(transform.position, targetDir, Color.green);
        if (Physics.Raycast(facingRay, out hit, DetectionDist))
        {

            if(hit.collider.tag == "Player" && Chasing) // jos ray collidee objektiin jolla on Player tag ja se on Detection distancen sisäpuolella
            {
                    movement.Chase();

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
        }
            
    }

}
