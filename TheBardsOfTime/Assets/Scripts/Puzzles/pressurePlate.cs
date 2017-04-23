using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class pressurePlate : MonoBehaviour {

    public int totalWeight = 5;
    public int currentWeight;
    public GameObject Obstacle;
    public List<GameObject> weights = new List<GameObject>();

    void OnTriggerEnter(Collider col) {
        GameObject temp = col.gameObject;

        if(temp.tag == "pickup" && !weights.Contains(temp)) {
            weights.Add(temp);
            checkWeights();
        }
    }

    void OnTriggerExit(Collider col) {
        GameObject temp = col.gameObject;
        if(temp.tag == "pickup" && weights.Contains(temp)) {
            weights.Remove(temp);
            checkWeights();
        }
    }

    void checkWeights() {

        currentWeight = 0;
        foreach (GameObject go in weights) {
            currentWeight += go.GetComponent<Pickupable>().weight;
        }

        if (totalWeight == currentWeight) {
            puzzleCompleted();
        }
    }

    void puzzleCompleted()
    {
        Obstacle.SetActive(false);
        GameManager gm = FindObjectOfType<GameManager>();
        gm.CompleteLevel();
    }
}
