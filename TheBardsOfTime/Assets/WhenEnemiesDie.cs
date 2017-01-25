using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WhenEnemiesDie : MonoBehaviour {

    private List<GameObject> EnemiesAlive = new List<GameObject>();

    void Update()
    {
        SearchForEnemies();

        if(EnemiesAlive.Count > 0) {
            gameObject.SetActive(true);
        } else {
            gameObject.SetActive(false);
        }
    }

    private void SearchForEnemies()
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Enemy");
        EnemiesAlive.Clear();
        foreach(GameObject go in temp) {
            EnemiesAlive.Add(go);
        }
    }
}
