using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using LitJson;

public class GameManager : MonoBehaviour
{
    Text noteCount;
    public int notes = 0;
    public float[] lastPos;
    static GameManager GM;
    public Dictionary<string, bool> levels = new Dictionary<string, bool>();
    public bool invertEnabled = false, freeCamEnabled = false;

    //Singleton
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

    void Save()
    {
        Debug.Log("Saving the game...");

        JsonData jeissoni;
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        PlayerData data = new PlayerData(
            player.GetComponent<HPScript>().hitpoints,
            notes,
            new double[] { player.transform.position.x, player.transform.position.y, player.transform.position.z },
            player.transform.eulerAngles.y,
            SceneManager.GetActiveScene().name
            );

        jeissoni = JsonMapper.ToJson(data);
        File.WriteAllText(Application.dataPath + "/Saves/Player.json", jeissoni.ToString());

        Debug.Log("Saving complete!");
    }

    void SaveLevel()
    {

    }

    void Load()
    {
        if (File.Exists(Application.dataPath + "/Saves/Player.json")) {
            Debug.Log("Loading the game...");

            string stringi = File.ReadAllText(Application.dataPath + "/Saves/Player.json");
            PlayerData data = JsonUtility.FromJson<PlayerData>(stringi);
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            player.GetComponent<HPScript>().hitpoints = data.health;
            notes = data.notes;
            lastPos = new float[] { (float)data.pos[0], (float)data.pos[1], (float)data.pos[2], (float)data.rot };
            Debug.Log("Loading Complete!");

            SceneManager.LoadScene(data.lastLevel);
        }
    }
}


class PlayerData
{
    public int health;
    public int notes;
    public double[] pos;
    public double rot;
    public string lastLevel;

    public PlayerData(int h, int n, double[] p, double r, string ll)
    {
        health = h; notes = n; pos = p;
        rot = r; lastLevel = ll;
    }
}

class SavableObject
{
    public string name;
    public double[] pos;
    public double rot;

    public SavableObject(string n, double[] p, double r)
    {
        name = n; pos = p; rot = r;
    }
}

class LevelState
{
    public string levelName;
    public bool completed;
    public SavableObject[] objects;

    public LevelState(string n, bool c, SavableObject[] o)
    {
        levelName = n; completed = c; objects = o;
    }
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
}*/
