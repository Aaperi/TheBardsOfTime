using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelLoader : MonoBehaviour {

    Object player;
    public int lvlID;

    void Start()
    {
        player = FindObjectOfType<CC>();
    }

    void OnTriggerEnter (Collider col)
    {
        if(col.gameObject.name == player.name) {
            SceneManager.LoadScene(lvlID);
        }
    }
}
