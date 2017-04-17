using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelLoader : MonoBehaviour {

    bool doNotTriggerMultipleTimesPlz = true;
    Object player;
    GameManager gm;
    public int lvlID;
    public bool Door;

    void Start()
    {
        player = FindObjectOfType<CC>();
        gm = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter (Collider col)
    {
        if(col.gameObject.name == player.name && doNotTriggerMultipleTimesPlz) {
            gm.UpdateLevel();
            doNotTriggerMultipleTimesPlz = false;
            gm.lastLevelID = Door ? SceneManager.GetActiveScene().buildIndex : 0;
            Debug.Log(gm.lastLevelID + " ID ennen telee, manager");
            Debug.Log(SceneManager.GetActiveScene().buildIndex + " ID ennen telee, buildindex");
            SceneManager.LoadScene(lvlID);
        }
    }
}
