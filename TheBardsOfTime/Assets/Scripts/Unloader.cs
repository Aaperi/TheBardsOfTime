using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Unloader : MonoBehaviour{

    void Start()
    {
        if (FindObjectOfType<GameManager>() != null) {
            GameManager gm = FindObjectOfType<GameManager>();
            if (!gm.levels.ContainsKey(SceneManager.GetActiveScene().name))
                gm.levels.Add(SceneManager.GetActiveScene().name, false);
            if (gm.levels[SceneManager.GetActiveScene().name])
                gameObject.SetActive(false);
        }
    }
}
