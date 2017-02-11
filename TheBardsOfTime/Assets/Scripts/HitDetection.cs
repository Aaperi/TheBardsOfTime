using UnityEngine;
using System.Collections.Generic;

public class HitDetection : MonoBehaviour
{

    public List<GameObject> enemyList = new List<GameObject>();
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Hitbox")) {
            GameObject temp = col.gameObject.transform.parent.gameObject;
            if (!enemyList.Contains(temp)) {
                enemyList.Add(temp);
                SortByDistance(enemyList);
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Hitbox")) {
            GameObject temp = col.gameObject.transform.parent.gameObject;
            if (enemyList.Contains(temp)) {
                enemyList.Remove(temp);
                SortByDistance(enemyList);
            }
        }
    }

    void SortByDistance(List<GameObject> list)
    {
        list.Sort(delegate (GameObject a, GameObject b) {
            float distA = Vector3.Distance(a.transform.position, player.transform.position);
            float distB = Vector3.Distance(b.transform.position, player.transform.position);
            return distA.CompareTo(distB);
        });
    }
}
