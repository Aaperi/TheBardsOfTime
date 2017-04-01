using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Savable : MonoBehaviour
{

    public bool dormant = true;
    public bool destroyOnLoad = false;

    void Start()
    {
        GameManager GM = FindObjectOfType<GameManager>();
        ObjectData data = GM.GetObjectData(gameObject.name);

        if (data != null) {

            Debug.Log(data.name + " " + data.dormant + " " + data.destroyOnLoad);

            if (!data.dormant) {
                transform.position = new Vector3((float)data.pos[0], (float)data.pos[1], (float)data.pos[2]);
                transform.eulerAngles = new Vector3(0, (float)data.rot, 0);
            }
            if (data.destroyOnLoad)
                Destroy(gameObject);
        } else
            Debug.Log("Data is null");
    }
}