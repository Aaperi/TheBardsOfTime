using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathScript : MonoBehaviour {

    public List<GameObject> Path = new List<GameObject>();
    public bool PathEnabled = true;

    public GameObject AddPoint()
    {
        GameObject obj = new GameObject();
        obj.transform.position = (Path.Count > 0) ?
            Path[Path.Count - 1].transform.position : transform.position;

        obj.transform.parent = this.transform;

        PathPointScript script = obj.AddComponent<PathPointScript>();
        script.Path = this;
        Path.Add(obj);

        obj.name = "p" + (Path.Count - 1);

        return obj;
    }

    public void RemovePoint(GameObject obj)
    {
        Path.Remove(obj);
        DestroyImmediate(obj);
        Destroy(obj);
    }


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDrawGizmos()
    {
        Gizmos.color = (PathEnabled) ? Color.red : Color.gray;
        Gizmos.DrawWireSphere(transform.position, 1f);

        Gizmos.color = (PathEnabled) ? Color.blue : Color.gray;
        Vector3 start = transform.position;
        foreach (GameObject obj in Path)
        {
            Gizmos.DrawLine(start, obj.transform.position);
            start = obj.transform.position;
        }
    }
}
