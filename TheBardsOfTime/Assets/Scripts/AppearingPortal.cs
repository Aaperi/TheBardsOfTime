using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class AppearingPortal : MonoBehaviour {

    Vector3 OGpos;

    void Start()
    {
        OGpos = transform.position;
    }

	void Update () {
        if (FindObjectOfType<StatePatternBoss>() != null)
            transform.position = new Vector3(OGpos.x, OGpos.y - 30, OGpos.z);
        else {
            transform.position = OGpos;
            GameManager gm = FindObjectOfType<GameManager>();
            gm.levels[SceneManager.GetActiveScene().name] = true;
            Debug.Log(gm.levels[SceneManager.GetActiveScene().name]);
        }
	}
}
