using UnityEngine;
using System.Collections.Generic;

public class HitDetection : MonoBehaviour {

    public List<GameObject> enemyList = new List<GameObject>();

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.layer == LayerMask.NameToLayer("Hitbox"))
        {
            GameObject temp = col.gameObject.transform.parent.gameObject;
            if (!enemyList.Contains(temp))
                enemyList.Add(temp);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Hitbox"))
        {
            GameObject temp = col.gameObject.transform.parent.gameObject;
            if (enemyList.Contains(temp))
                enemyList.Remove(temp);
        }
    }
}
