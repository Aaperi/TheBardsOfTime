using UnityEngine;
using System.Collections;

public class PathPointScript : MonoBehaviour {

    public PathScript Path;
    public string PointName
    {
        get
        {
            string name = "P" + Path.Path.IndexOf(this.gameObject);
            this.gameObject.name = name;
            return name;
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDrawGizmos()
    {
        Gizmos.color = (Path.PathEnabled) ? Color.green : Color.grey;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}
