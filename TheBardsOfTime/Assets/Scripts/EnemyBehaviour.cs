using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {
    public float fieldOfView = 115f;
    public bool playerInSight;
    public Vector3 personalLastSighting;

    private Transform Target;
    private NavMeshAgent nav;
    private EnemyMovement movement;
    private GameObject player;
    private SphereCollider Sphere;
    private Vector3 previousSighting;
    private LastPlayerSighting lastPlayerSighting;

    // Use this for initialization
    void Start() {
        Target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        movement = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyMovement>();
        player = GameObject.FindGameObjectWithTag("Player");
        Sphere = GetComponent<SphereCollider>();
        nav = GetComponent<NavMeshAgent>();
        lastPlayerSighting = FindObjectOfType<LastPlayerSighting>();

        personalLastSighting = lastPlayerSighting.resetPosition;
        previousSighting = lastPlayerSighting.resetPosition;
    }

    // Update is called once per frame
    void Update() {
        /* Vector3 targetDir = Target.position - transform.position;
         RaycastHit hit;
         Ray facingRay = new Ray(transform.position, targetDir); // ray vihollisesta pelaajaan
         Debug.DrawRay(transform.position, targetDir, Color.green);

         if (Roaming)
         {
             movement.Patrol();
         }

         if (Physics.Raycast(facingRay, out hit, DetectionDist))
         {

             if (hit.collider.tag == "Player" && Chasing) // jos ray collidee objektiin jolla on Player tag ja se on Detection distancen sisäpuolella
             {
                 movement.Chase();
             }
         }*/
        
        if (lastPlayerSighting.position != previousSighting)
            personalLastSighting = lastPlayerSighting.position;

        previousSighting = lastPlayerSighting.position;
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject == player)
        {
            playerInSight = false;

            float dist = Vector3.Distance(transform.position, col.gameObject.transform.position);
            Vector3 direction = player.gameObject.transform.position - transform.position;
            float angle = Vector3.Angle(direction.normalized * dist, transform.forward);
            Debug.DrawRay(transform.position, direction.normalized * dist, Color.red);

            if (angle < fieldOfView * 0.5f)
            {
                RaycastHit hit;
                if(Physics.Raycast(transform.position, direction.normalized * dist, out hit, Sphere.radius))
                {
                    if(hit.collider.gameObject == player)
                    {
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction, Vector3.up), 0.1f);
                        playerInSight = true;
                        lastPlayerSighting.position = col.transform.position;
                    }
                }
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject == player)
            playerInSight = false;

    }

    float CalculatePathLength(Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();
        if (nav.enabled)
            nav.CalculatePath(targetPosition, path);

        Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];
        allWayPoints[0] = transform.position;
        allWayPoints[allWayPoints.Length + 1] = targetPosition;

        for (int i = 0; i < path.corners.Length; i++)
        {
            allWayPoints[i + 1] = path.corners[i];
        }

        float pathLength = 0;

        for(int i = 0; i < allWayPoints.Length - 1; i++)
        {
            pathLength += Vector3.Distance(allWayPoints[i], allWayPoints[i + 1]);
        }

        return pathLength;
    }
}
