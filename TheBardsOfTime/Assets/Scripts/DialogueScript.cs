using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using DialogClass;

public class DialogueScript : MonoBehaviour {
    [HideInInspector]
    //public Canvas chat;
    public Sprite speaker1, speaker2;
    public Image dialogImage;
    public float dialogueSpeed = 0f;
    [HideInInspector]
    public Image portrait;

    private Text text;
    private Dialogue dia;
    private UIPanel uiPanel;

	[HideInInspector]
    public bool dialogueActive;

	// Use this for initialization
	void Start () {
        LoadDialogues();
        uiPanel = FindObjectOfType<UIPanel>();
        dialogueActive = false;
        //chat = GameObject.Find("chat").GetComponent<Canvas>();
        text = uiPanel.diaText;
        portrait = uiPanel.diaImage;
        //chat.enabled = false;
	}

    void Speaker1(string dialogue) {
        portrait.sprite = speaker1;
        text.text = dialogue;
    }

    void Speaker2(string dialogue) {
        portrait.sprite = speaker2;
        text.text = dialogue;
    }

    private void LoadDialogues()
    {
        XmlSerializer serial = new XmlSerializer(typeof(Dialogue));
        Stream reader = new FileStream("Assets/Resources/Data/dialogs.xml", FileMode.Open);
        dia = (Dialogue)serial.Deserialize(reader);
    }

    public IEnumerator dialogFromXml(int sceneID)
    {
        //chat.enabled = true;
        if (!dialogueActive) {
            dialogueActive = true;
            foreach (Line ln in dia.Scenes[sceneID].Lines) {
                if(ln.SpeakerID == 1)
                    Speaker1(ln.Text);
                else
                    Speaker2(ln.Text);
                yield return StartCoroutine(WaitForTime(ln.Delay));
            }
        }
        dialogueActive = false;
        //chat.enabled = false;
    }

    IEnumerator WaitForTime(float time)
    {
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < start + time) {
            if (Input.GetKeyDown(KeyCode.E) && dialogueActive) {
                start = Time.realtimeSinceStartup;
                while (Time.realtimeSinceStartup < start + .2f) {
                    yield return null;
                }
                break;
            } else yield return null;
        }
    }
}
