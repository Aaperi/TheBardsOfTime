using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelLoader : MonoBehaviour {

    Object player;

    void Start()
    {
        player = FindObjectOfType<CC>();
    }

    void OnTriggerEnter (Collider col)
    {
        if(col.gameObject.name == player.name) {
            SceneManager.LoadScene(int.Parse(gameObject.name));
        }
    }
}
