using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetManager : MonoBehaviour {

    private CC CCref;
    private int pointer = 0;
    public List<GameObject> enemyList = new List<GameObject>();

    void Start()
    {
        CCref = GameObject.Find("Player").GetComponent<CC>();
    }

    void Update()
    {
        Vector3 newRotation = transform.eulerAngles;
        newRotation.y = Camera.main.transform.eulerAngles.y + 90;
        transform.eulerAngles = newRotation;
    }

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

    public void RemoveTarget(GameObject go)
    {
        if (CCref.target == go) {
            CCref.targetIsLocked = false;
        }
        enemyList.Remove(go);
    }

    void SortByDistance(List<GameObject> list)
    {
        list.Sort(delegate (GameObject a, GameObject b) {
            float distA = Vector3.Distance(a.transform.position, transform.position);
            float distB = Vector3.Distance(b.transform.position, transform.position);
            return distA.CompareTo(distB);
        });
    }

    public void changeTarget(string who)
    {
        switch (who) {
            case "First": {
                    pointer = 0;
                    if (enemyList.Count > 0) {
                        SortByDistance(enemyList);
                        CCref.target = enemyList[pointer];
                    } else
                        CCref.target = null;
                    break;
                }

            case "Next": {
                    pointer += 1;
                    if (pointer < enemyList.Count && enemyList.Count > 0) {
                        CCref.target = enemyList[pointer];
                    } else {
                        pointer = 0;
                        CCref.target = enemyList[pointer];
                    }
                    break;
                }

            default: CCref.targetIsLocked = false; break;
        }
    }
}