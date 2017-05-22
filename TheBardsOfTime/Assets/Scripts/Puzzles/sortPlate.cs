using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class sortPlate : MonoBehaviour {
    public GameObject Portal;
    public bool isComplete = false;
    public GameObject[] neededPiece;

    private int succesrate = 0;
    private sortPlate[] otherPlates;
    private List<GameObject> stuff = new List<GameObject>();
    private GameManager gm;

    void Start() {
        otherPlates = FindObjectsOfType<sortPlate>();
        gm = FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter(Collider col) {
        if (col.gameObject.CompareTag("pickup")) {
            GameObject temp = col.gameObject;

            if (!stuff.Contains(temp))
                stuff.Add(temp);


            for (int i = 0; i < stuff.Count; i++) {
                for (int e = 0; e < neededPiece.Length; e++) {
                    if (stuff[i].name == neededPiece[e].name)
                        isComplete = true;
                    else
                        isComplete = false;


                    Debug.Log(stuff[i].name + " = " + neededPiece[e].name);
                    Debug.Log(otherPlates.Length + " " + succesrate);
                }

            }

        }

        sortCheck();
        /* if (stuff.Count == 1 && gameObject.name.Contains(stuff[0].name))
             isComplete = true;
         else
             isComplete = false;*/
    }

    void OnTriggerExit(Collider col) {
        if (col.gameObject.CompareTag("pickup")) {
            GameObject temp = col.gameObject;

            if (stuff.Contains(temp))
                stuff.Remove(temp);

            for (int i = 0; i < stuff.Count; i++) {
                for (int e = 0; e < neededPiece.Length; e++) {
                    if (temp.name == neededPiece[e].name)
                        isComplete = true;
                    else
                        isComplete = false;
                }

            }
        }

        /*
        if (stuff.Count == 1 && gameObject.name.Contains(stuff[0].name))
            isComplete = true;
        else
            isComplete = false;*/

        sortCheck();
    }

    void sortCheck() {
        foreach (sortPlate sp in otherPlates) {
            if (sp.isComplete)
                succesrate++;
            else
                succesrate = 0;
        }
        if (succesrate >= otherPlates.Length) {
            Debug.Log(otherPlates.Length + " " + succesrate);
            try { puzzleCompleted(); } catch { }
        }
    }

    void puzzleCompleted() {
        Debug.Log("Puzzle done");
        Portal.SetActive(true);

        //gm.levels[SceneManager.GetActiveScene().name] = true;
        //Debug.Log(gm.levels[SceneManager.GetActiveScene().name]);
    }
}