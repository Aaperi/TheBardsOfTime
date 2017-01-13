using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetDetection : MonoBehaviour {

    public List<GameObject> enemyList = new List<GameObject>();

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Hitbox")) {
            GameObject temp = col.gameObject.transform.parent.gameObject;
            if (!enemyList.Contains(temp))
                enemyList.Add(temp);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Hitbox")) {
            GameObject temp = col.gameObject.transform.parent.gameObject;
            if (enemyList.Contains(temp))
                enemyList.Remove(temp);
        }
    }

    void WhoIsTheClosestOne()
    {
        enemyList.Sort(delegate (GameObject a, GameObject b) {
            float distA = Vector3.Distance(a.transform.position, transform.position);
            float distB = Vector3.Distance(b.transform.position, transform.position);
            return distA.CompareTo(distB);
        });

        foreach(GameObject go in enemyList) {
            Debug.Log(Vector3.Distance(go.transform.position, transform.position));
        }
    }
}
