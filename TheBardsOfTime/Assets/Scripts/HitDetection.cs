using UnityEngine;
using System.Collections.Generic;

public class HitDetection : MonoBehaviour {

    private List<GameObject> colliders = new List<GameObject>();
    private string updateAlert;

    void Start()
    {
        updateAlert = gameObject.name + "RangeUpdate";
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
