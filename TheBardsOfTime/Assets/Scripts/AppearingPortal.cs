using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class AppearingPortal : MonoBehaviour {

    Vector3 OGpos;

    void Start()
    {
        OGpos = transform.position;
        transform.position = new Vector3(OGpos.x, OGpos.y + 60, OGpos.z);
    }

	void Update () {
        if (FindObjectOfType<StatePatternBoss>() != null)
            transform.position = new Vector3(OGpos.x, OGpos.y - 60, OGpos.z);
	}
}
