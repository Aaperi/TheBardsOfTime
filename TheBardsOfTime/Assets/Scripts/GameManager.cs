using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameManager : MonoBehaviour
{
    public int notes = 0;
    public int health = 0;
    static GameManager gm;
    public Dictionary<string, bool> levels = new Dictionary<string, bool>();
    public bool invertEnabled = false, freeCamEnabled = false;

    public static GameManager Instance {
        get {
            if (gm == null) {
                gm = new GameObject("GameManager").AddComponent<GameManager>();
            }
            return gm;
        }
    }

    void Awake()
    {
        if (gm == null) {
            gm = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (!levels.ContainsKey(SceneManager.GetActiveScene().name))
            levels.Add(SceneManager.GetActiveScene().name, false);
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);

        PlayerData data = new PlayerData();
        data.health = health;
        data.notes = notes;

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            health = data.health;
            notes = data.notes;
        }
    }

    [SerializeField]
    class PlayerData
    {
        public int health;
        public int notes;
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
