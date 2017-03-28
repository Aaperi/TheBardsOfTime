using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    Text noteCount;
    public int notes = 0;
    public int health = 0;
    public Transform lastPos;
    static GameManager GM;
    public Dictionary<string, bool> levels = new Dictionary<string, bool>();
    public bool invertEnabled = false, freeCamEnabled = false;

    void Awake()
    {
        if (GM == null) {
            GM = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (!levels.ContainsKey(SceneManager.GetActiveScene().name))
            levels.Add(SceneManager.GetActiveScene().name, false);

        Text noteCount = GameObject.Find("noteCount").GetComponent<Text>();
        noteCount.text = noteCount.text.Substring(0, 7) + notes;

        if (Input.GetKeyDown(KeyCode.Keypad0)) {
            Save();
        }

        if (Input.GetKeyDown(KeyCode.Keypad7)) {
            Load();
        }
    }

    public void Save()
    {
        Debug.Log("Saving the game...");
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");

        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        PlayerData data = new PlayerData();
        data.health = health;
        data.notes = notes;
        data.xPos = player.position.x;
        data.yPos = player.position.y;
        data.zPos = player.position.z;
        data.rot = player.rotation.y;
        data.lastLevel = SceneManager.GetActiveScene().name;
        data.levels = levels;

        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Saving complete!");
    }

    public void Load()
    {
        Debug.Log("Loading the game...");
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            health = data.health;
            notes = data.notes;
            lastPos = transform;
            lastPos.position = new Vector3(data.xPos, data.yPos, data.zPos);
            lastPos.eulerAngles = new Vector3(0, data.rot, 0);
            Debug.Log("Loading Complete!");
            SceneManager.LoadScene(data.lastLevel);
        }
    }

    [Serializable]
    class PlayerData
    {
        public int health;
        public int notes;
        public float xPos;
        public float yPos;
        public float zPos;
        public float rot;
        public string lastLevel;
        public Dictionary<string, bool> levels;
    }

    [Serializable]
    class LevelState
    {
        public string savedItems;
    }

    /*void getSceneNames()
    {

        try {
            foreach (UnityEditor.EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes) {
                string name = S.path.Substring(S.path.LastIndexOf('/') + 1);
                name = name.Substring(0, name.Length - 6);
                levels.Add(name, false);
            }
        } catch { }
    }*/
}
