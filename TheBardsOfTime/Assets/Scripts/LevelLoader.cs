using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelLoader : MonoBehaviour
{
    Object player;
    GameManager gm;
    public int lvlID;
    public bool Door;
    bool stop = false;

    void OnTriggerEnter(Collider col)
    {
        player = FindObjectOfType<CC>();
        if (col.gameObject.name == player.name && !stop) {
            gm = FindObjectOfType<GameManager>();
            gm.UpdateLevel();
            stop = true;
            gm.lastLevelID = Door ? SceneManager.GetActiveScene().buildIndex : 0;
            Debug.Log(gm.lastLevelID + " ID ennen telee, manager");
            Debug.Log(SceneManager.GetActiveScene().buildIndex + " ID ennen telee, buildindex");
            SceneManager.LoadScene(lvlID);
        }
    }
}
