using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyBehaviour : MonoBehaviour {
    public float fieldOfView = 115f;
    public bool playerInSight;
    public Vector3 personalLastSighting;
    public float dist;

    private List<GameObject> detected = new List<GameObject>();
    private GameObject player;
    private SphereCollider Sphere; // Ei käytössä atm, ehkä voisi muokata ilmotusrangeksi kun jahtaa vihollista?
    private Vector3 previousSighting;
    private LastPlayerSighting lastPlayerSighting;
    private bool withinRange = false;

    // Use this for initialization
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        Sphere = GetComponent<SphereCollider>();
        lastPlayerSighting = FindObjectOfType<LastPlayerSighting>();

        personalLastSighting = lastPlayerSighting.resetPosition;
        previousSighting = lastPlayerSighting.resetPosition;
    }

    // Update is called once per frame
    void Update() {
        dist = Vector3.Distance(transform.position, player.transform.position);
        Vector3 direction = player.transform.position - transform.position;
        float angle = Vector3.Angle(direction.normalized * dist, transform.forward);
        Debug.DrawRay(transform.position, direction.normalized * dist, Color.red);

        if (angle < fieldOfView * 0.5f)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction.normalized, out hit, dist))
            {
                if (hit.collider.tag == "Player" && withinRange)
                {
                    playerInSight = true;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction, Vector3.up), 0.1f);
                    lastPlayerSighting.position = player.transform.position;
                }
            }
        }

        for(int i = 0; i < detected.Count; i++)
        {
            if(detected[i].tag == "Player")
            {
                if (lastPlayerSighting.position != previousSighting)
                    personalLastSighting = lastPlayerSighting.position;

                previousSighting = lastPlayerSighting.position;
            }
        }
        

    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player" || col.tag == "Enemy")
        {
            detected.Add(col.gameObject);
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject == player)
        {
            withinRange = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player" || col.tag == "Enemy")
        {
            detected.Remove(col.gameObject);
        }

        if (col.gameObject == player)
        {
            withinRange = false;
        }
    }

}
