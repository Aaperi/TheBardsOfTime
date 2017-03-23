using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetManager : MonoBehaviour
{

    public float range = 60;
    public float radius = 120;
    public List<GameObject> enemyList;

    void Update()
    {
        enemyList = new List<GameObject>();
        List<StatePatternEnemy> temp = new List<StatePatternEnemy>(FindObjectsOfType<StatePatternEnemy>());
        if (temp.Count != enemyList.Count) {
            foreach (StatePatternEnemy S in temp) {
                if (HitCheck(S.gameObject))
                    enemyList.Add(S.gameObject);
            }
        }
    }

    bool HitCheck(GameObject target)
    {
        if (Vector3.Distance(transform.position, target.transform.position) <= range) {
            Vector3 targetDir = target.transform.position - transform.position;
            if (Vector3.Angle(targetDir, transform.forward) <= radius / 2) {
                return true;
            } else
                return false;
        } else
            return false;
    }

    void SortByDistance(List<GameObject> list)
    {
        list.Sort(delegate (GameObject a, GameObject b) {
            float distA = Vector3.Distance(a.transform.position, transform.position);
            float distB = Vector3.Distance(b.transform.position, transform.position);
            return distA.CompareTo(distB);
        });
    }

    public GameObject getTarget(string FirstOrAnother)
    {
        int pointer = 0;
        switch (FirstOrAnother) {
            case "First": {
                if (enemyList.Count > 0) {
                    SortByDistance(enemyList);
                    return enemyList[0];
                } else
                    return null;
            }

            case "Another": {
                pointer += 1;
                if (pointer < enemyList.Count && enemyList.Count > 0) {
                    return enemyList[pointer];
                } else {
                    pointer = 0;
                    return enemyList[pointer];
                }
            }

            default: return null;
        }
    }
}