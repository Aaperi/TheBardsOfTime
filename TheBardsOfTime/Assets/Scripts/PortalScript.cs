using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PortalScript : MonoBehaviour
{
    Object player;
    GameManager gm;
    public int lvlID;
    public bool Door;
    public string[] Requirements;
    bool stop = false;

    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        bool check = true;

        if (Requirements.Length > 0)
            foreach (string lvl in Requirements) {
                LevelState ls = gm.GetLevelState(lvl);
                if (ls == null)
                    check = false;
                else {
                    if (!ls.completed)
                        check = false;
                }
            }

        if (!check)
            gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider col)
    {
        player = FindObjectOfType<CC>();
        if (col.gameObject.name == player.name && !stop) {
            gm.UpdateLevel();
            stop = true;
            gm.lastLevelID = Door ? SceneManager.GetActiveScene().buildIndex : 0;
            Debug.Log(gm.lastLevelID + " ID ennen telee, manager");
            Debug.Log(SceneManager.GetActiveScene().buildIndex + " ID ennen telee, buildindex");
            SceneManager.LoadScene(lvlID);
        }
    }
}
