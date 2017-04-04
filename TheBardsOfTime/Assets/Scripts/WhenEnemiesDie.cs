using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class WhenEnemiesDie : MonoBehaviour {

    private List<GameObject> EnemiesAlive = new List<GameObject>();

    void Update()
    {
        SearchForEnemies();

        if(EnemiesAlive.Count > 0) {
            gameObject.SetActive(true);
        } else {
            sceneComplete();
        }
    }

    void SearchForEnemies()
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Enemy");
        EnemiesAlive.Clear();
        foreach(GameObject go in temp) {
            EnemiesAlive.Add(go);
        }
    }

    void sceneComplete()
    {
        gameObject.SetActive(false);
        GameManager gm = FindObjectOfType<GameManager>();
        //gm.levels[SceneManager.GetActiveScene().name] = true;
        //Debug.Log(gm.levels[SceneManager.GetActiveScene().name]);
    }
}
