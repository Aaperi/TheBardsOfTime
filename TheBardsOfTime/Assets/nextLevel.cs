using UnityEngine;
using UnityEngine.SceneManagement;

public class nextLevel : MonoBehaviour {

    void OnTriggerEnter(Collider col) {
        if (col.gameObject.name == "Player")
            SceneManager.LoadScene(1);
    }
}
