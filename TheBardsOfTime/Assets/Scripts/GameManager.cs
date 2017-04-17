using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using LitJson;

public class GameManager : MonoBehaviour
{
    public Text noteCount;
    List<LevelState> leveltemps = new List<LevelState>();
    static GameManager GM;
    UIPanel uiPanel;
    CC cc;
    DialogueScript dia;
    public int playerHp;
    public int notes = 0;
    public int lastLevelID;
    public float[] lastPos;
    public bool invertEnabled = false, freeCamEnabled = false, paused = false;

    //Singleton
    void Awake()
    {
        uiPanel = FindObjectOfType<UIPanel>();
        cc = FindObjectOfType<CC>();
        dia = FindObjectOfType<DialogueScript>();

        if (GM == null) {
            GM = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        try {
            Text noteCount = GameObject.Find("noteCount").GetComponent<Text>();
            noteCount.text = noteCount.text.Substring(0, 7) + notes;
        } catch { }  
    }

    public void Save()
    {
        //Tehään playerdata
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerData data = new PlayerData(
            player.GetComponent<HPScript>().hitpoints,
            notes,
            new double[] { player.transform.position.x, player.transform.position.y, player.transform.position.z },
            player.transform.eulerAngles.y,
            SceneManager.GetActiveScene().name
            );

        //Tallennetaan Playerdata
        JsonData jeissoni;
        jeissoni = JsonMapper.ToJson(data);
        File.WriteAllText(Application.dataPath + "/Saves/Player.json", jeissoni.ToString());

        //Tallennetaan kaikki levelstatet
        foreach (LevelState ls in leveltemps) {
            jeissoni = JsonMapper.ToJson(ls);
            File.WriteAllText(Application.dataPath + "/Saves/" + SceneManager.GetActiveScene().name + ".json", jeissoni.ToString());
        }

        Debug.Log("Player saved!");
    }

    public void UpdateLevel()
    {
        List<ObjectData> objects = new List<ObjectData>();

        //Katotaan jos kyseisestä kentästä on jo tietoja tallella ja ladataan ne
        for (int k = 0; k < leveltemps.Count; k++)
            if (leveltemps[k].levelName == SceneManager.GetActiveScene().name) {
                objects.AddRange(leveltemps[k].objects);
                foreach (Savable s in FindObjectsOfType<Savable>()) {
                    GameObject go = s.gameObject;
                    for (int t = 0; t < objects.Count; t++)
                        if (objects[t].name == go.name) {
                            ObjectData od = new ObjectData(
                                go.name,
                                new double[] { go.transform.position.x, go.transform.position.x, go.transform.position.x },
                                go.transform.rotation.eulerAngles.y,
                                go.GetComponent<Savable>().dormant,
                                go.GetComponent<Savable>().destroyOnLoad
                            );
                            objects[t] = od;
                        }
                }
            }

        if (objects.Count == 0) {

            //Haetaan kaikki gameobjectit jokka pitää tallentaa
            List<GameObject> temps = new List<GameObject>();
            foreach (Savable s in FindObjectsOfType<Savable>())
                temps.Add(s.gameObject);

            //Tehään lista gameobjectien tiedoista
            foreach (GameObject go in temps) {
                ObjectData od = new ObjectData(
                    go.name,
                    new double[] { go.transform.position.x, go.transform.position.x, go.transform.position.x },
                    go.transform.rotation.eulerAngles.y,
                    go.GetComponent<Savable>().dormant,
                    go.GetComponent<Savable>().destroyOnLoad
                    );
                objects.Add(od);
            }
        }

        //Tehdään levelstate
        LevelState state = new LevelState(
            SceneManager.GetActiveScene().name,
            false,
            objects.ToArray()
            );

        //tallennetaan tään hetkinen kenttä temppitaulukkoon
        if (leveltemps.Count > 0)
            for (int k = 0; k < leveltemps.Count; k++)
                if (leveltemps[k].levelName == SceneManager.GetActiveScene().name)
                    leveltemps[k] = state;
                else
                    leveltemps.Add(state);
        else
            leveltemps.Add(state);

        Debug.Log("Level Updated!");
    }

    public void Load()
    {
        if (File.Exists(Application.dataPath + "/Saves/Player.json")) {

            //Haetaan pelaajan tiedot ja tallenetaan ne uuteen classiin
            string stringi = File.ReadAllText(Application.dataPath + "/Saves/Player.json");
            PlayerData data = JsonUtility.FromJson<PlayerData>(stringi);
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            //Tehdään tallennustiedoilla juttuja
            player.GetComponent<HPScript>().hitpoints = data.health;
            notes = data.notes;
            lastPos = new float[] { (float)data.pos[0], (float)data.pos[1], (float)data.pos[2], (float)data.rot };
            leveltemps.Clear();
            lastLevelID = 0;
            Debug.Log("Loading Complete!");

            SceneManager.LoadScene(data.lastLevel);
        }
    }

    public ObjectData GetObjectData(string name)
    {
        ObjectData data = null;

        //Kattoo jos temppilistasta löytyy kysytty objekti
        for (int k = 0; k < leveltemps.Count; k++)
            for (int t = 0; t < leveltemps[k].objects.Length; t++)
                if (leveltemps[k].objects[t].name == name)
                    data = leveltemps[k].objects[t];

        //Jos ei löytyny tempistä niin katotaan jos löytys tallennusfilusta
        if (File.Exists(Application.dataPath + "/Saves/" + SceneManager.GetActiveScene().name + ".json") && data == null) {

            //Haetaan kentän tiedot ja tallenetaan ne uuteen classiin
            string stringi = File.ReadAllText(Application.dataPath + "/Saves/" + SceneManager.GetActiveScene().name + ".json");
            LevelState level = JsonUtility.FromJson<LevelState>(stringi);

            //jos on olemassa taulukko niin sieltä haetaan oikea objekti
            if (level.objects.Length > 0)
                foreach (ObjectData od in level.objects)
                    if (od.name.Contains(name))
                        data = od;
        }

        return data;
    }

    public bool GetLevelState()
    {
        return false;
    }

    public void PauseGame() {
        paused = true;
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

class LevelState
{
    public string levelName;
    public bool completed;
    public ObjectData[] objects;

    public LevelState(string n, bool c, ObjectData[] o)
    {
        levelName = n; completed = c; objects = o;
    }
}



























/*void getSceneNames()
{
        foreach (UnityEditor.EditorBuildSettingsScene S in UnityEditor.EditorBuildSettings.scenes) {
            string name = S.path.Substring(S.path.LastIndexOf('/') + 1);
            name = name.Substring(0, name.Length - 6);
            levels.Add(name, false);
}*/
