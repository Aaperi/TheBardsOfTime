using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class pickUpObject : MonoBehaviour
{

    bool carrying;
    float smooth;
    public float range = 6;
    public float radius = 90;
    public float strength = 200;

    List<GameObject> objects = new List<GameObject>();
    GameObject carriedObject;
    UIActions UIAref;
	UIPanel UIPref;

    void Start()
    {
		UIAref = FindObjectOfType<UIActions>();
		UIPref = FindObjectOfType<UIPanel>();
        Pickupable[] temp = FindObjectsOfType<Pickupable>();
        foreach (Pickupable p in temp)
            objects.Add(p.gameObject);
    }

    void Update()
    {

        //tekee listan jossa on kaikki rangella olevat kannettavat jutut
        List<GameObject> temp = new List<GameObject>();
        foreach (GameObject g in objects)
            if (HitCheck(g))
                temp.Add(g);
        temp.Sort(delegate (GameObject a, GameObject b) {
            float distA = Vector3.Distance(a.transform.position, transform.position);
            float distB = Vector3.Distance(b.transform.position, transform.position);
            return distA.CompareTo(distB);
        });

        //kattoo ettÃ¤ millonka pitÃ¤Ã¤ olla viesti ruudulla
		if (temp.Count > 0 && !carrying && !carriedObject && !UIPref.actionGuide.gameObject.activeSelf)
			UIAref.SendMessage("ShowGuide", "pickup " + temp[0].name);
        else if (carrying)
			UIAref.SendMessage("HideGuide");

        if (Input.GetKeyDown(KeyCode.E)) {
            if (temp.Count > 0 && !carrying && carriedObject == null)
                PickUp(temp[0]);
            else if (carriedObject != null)
                Drop();
        }

        if (carrying && carriedObject != null)
            Carry();
    }


    void Carry()
    {
        if (GameObject.Find("pickupPlace")) {
            Vector3 temp = GameObject.Find("pickupPlace").transform.position;
            carriedObject.transform.position = new Vector3(temp.x, temp.y, temp.z);
        }
    }

    void Drop()
    {
        carrying = false;
        carriedObject.GetComponent<Rigidbody>().isKinematic = false;
        carriedObject.GetComponent<Rigidbody>().AddForce(transform.forward * strength);
        carriedObject = null;
    }

    void PickUp(GameObject target)
    {
        carrying = true;
        carriedObject = target;
        carriedObject.GetComponent<Rigidbody>().isKinematic = true;
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
}

