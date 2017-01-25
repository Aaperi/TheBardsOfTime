using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueScript : MonoBehaviour {
    [HideInInspector]
    public Canvas chat;
    public Sprite speaker1, speaker2;
    public Image dialogImage;
    public float dialogueSpeed = 0f;
    [HideInInspector]
    public Image portrait;

    private Text text;
    private MenuScript menu;

    [HideInInspector]
    public bool dialogueActive;

	// Use this for initialization
	void Start () {
        dialogueActive = false;
        chat = GameObject.Find("chat").GetComponent<Canvas>();
        text = chat.GetComponentInChildren<Text>();
        menu = FindObjectOfType<MenuScript>();
        portrait = chat.GetComponentInChildren<Image>();
        chat.enabled = false;
	}

    IEnumerator WaitForTime(float time) {
        float start = Time.realtimeSinceStartup;
        while(Time.realtimeSinceStartup < start + time) {
            if(Input.GetKeyDown(KeyCode.E) && dialogueActive) {
                start = Time.realtimeSinceStartup;
                while (Time.realtimeSinceStartup < start + .2f) {
                    yield return null;
                }
                break;
            }
            else yield return null;
        }
    }

    void Speaker1(string dialogue) {
        portrait.sprite = speaker1;
        text.text = dialogue;
    }

    void Speaker2(string dialogue) {
        portrait.sprite = speaker2;
        text.text = dialogue;
    }

    public IEnumerator dialogTest1(float delay) {
        chat.enabled = true;
        if (!dialogueActive) {
            dialogueActive = true;

            Speaker2("Hyvää päivää Seikkalija");
            yield return StartCoroutine(WaitForTime(2f + dialogueSpeed));

            Speaker2("Huomaan että teillä on mahtipisteet lopussa");
            yield return StartCoroutine(WaitForTime(3f + dialogueSpeed));

            Speaker1("Kyllä vain, olisiko teillä myydä taikajuomaa?");
            yield return StartCoroutine(WaitForTime(3f + dialogueSpeed));

            Speaker2("Kyllä vain, 23 kultarahaa");
            yield return StartCoroutine(WaitForTime(2f + dialogueSpeed));

            Speaker1("Kiitos!");
            yield return StartCoroutine(WaitForTime(1f + dialogueSpeed));

            Speaker2("Eipä kestä. Olisiko mahdollista udella, minne olette matkalla?");
            yield return StartCoroutine(WaitForTime(3f + dialogueSpeed));

            Speaker1("Sheikkailemaan :DDD");
            yield return StartCoroutine(WaitForTime(3f + dialogueSpeed));

            Speaker2("Sheikkailu on parasta :-D ja taikajuomat :-DDD");
            yield return StartCoroutine(WaitForTime(3f + dialogueSpeed));

            Speaker1("Joo joo joo!");
            yield return StartCoroutine(WaitForTime(3f + dialogueSpeed));
        }
        dialogueActive = false;
        chat.enabled = false;
    }
}
