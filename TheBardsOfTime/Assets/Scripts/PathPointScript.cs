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

    void OnDrawGizmos()
    {
        Gizmos.color = (Path.PathEnabled) ? Color.green : Color.gray;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}
