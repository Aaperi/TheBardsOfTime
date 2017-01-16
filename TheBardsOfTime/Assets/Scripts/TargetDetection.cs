using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetDetection : MonoBehaviour {

    public List<GameObject> enemyList = new List<GameObject>();
    public TargetManager tam;

    void Start()
    {
        tam = transform.parent.gameObject.GetComponent<TargetManager>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Hitbox")) {
            GameObject temp = col.gameObject.transform.parent.gameObject;
            if (!enemyList.Contains(temp))
                enemyList.Add(temp);
            SortandSend(enemyList);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Hitbox")) {
            GameObject temp = col.gameObject.transform.parent.gameObject;
            if (enemyList.Contains(temp))
                enemyList.Remove(temp);
            SortandSend(enemyList);
        }
    }

    void SortandSend(List<GameObject> list)
    {
        list.Sort(delegate (GameObject a, GameObject b) {
            float distA = Vector3.Distance(a.transform.position, transform.position);
            float distB = Vector3.Distance(b.transform.position, transform.position);
            return distA.CompareTo(distB);
        });
        tam.updateList(gameObject.name, enemyList);
    }
}
