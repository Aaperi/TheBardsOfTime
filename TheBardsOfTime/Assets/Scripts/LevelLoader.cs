using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelLoader : MonoBehaviour {

    Object player;
    GameManager gm;
    public int lvlID;

    void Start()
    {
        player = FindObjectOfType<CC>();
        gm = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter (Collider col)
    {
        if(col.gameObject.name == player.name) {
            gm.UpdateLevel();
            gm.lastLevelID = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(lvlID);
        }
    }
}
