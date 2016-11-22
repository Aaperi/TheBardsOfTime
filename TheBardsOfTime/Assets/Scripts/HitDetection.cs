using UnityEngine;
using System.Collections.Generic;

public class HitDetection : MonoBehaviour {

    private List<GameObject> colliders = new List<GameObject>();
    private string updateAlert;

    void Start()
    {
        updateAlert = gameObject.name + "RangeUpdate";
    }

    public void iDied(GameObject go)
    {
        Debug.Log("JOOJOOJOO");
        if (colliders.Contains(go)){
            foreach(GameObject gg in colliders)
            {
                Debug.Log(gg.name);
            }
            colliders.Remove(go);
            SendMessageUpwards(updateAlert, colliders);
            foreach (GameObject gg in colliders)
            {
                Debug.Log(gg.name);
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        GameObject temp = col.gameObject;
        if (!colliders.Contains(temp) && temp.layer == LayerMask.NameToLayer("Hitbox")){
            colliders.Add(temp);
            SendMessageUpwards(updateAlert, colliders);
        }
    }

    void OnTriggerExit(Collider col)
    {
        GameObject temp = col.gameObject;
        if (colliders.Contains(temp) && temp.layer == LayerMask.NameToLayer("Hitbox")){
            colliders.Remove(temp);
            SendMessageUpwards(updateAlert, colliders);
        }
    }
}
