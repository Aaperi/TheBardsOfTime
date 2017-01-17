using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueScript : MonoBehaviour {

    Text text;
    public Canvas chat;
    public float dialogueSpeed = 0f;

    private float elapsedTime;

	// Use this for initialization
	void Start () {
        text = chat.GetComponentInChildren<Text>();

        chat.enabled = false;
	}
	
	IEnumerator WaitForTime(float time) {
        elapsedTime = 0f;
        while(elapsedTime < time) {
            elapsedTime += Time.deltaTime;
            if(Input.GetKey(KeyCode.Space)) {
                yield return new WaitForSeconds(0.2f);
                break;
            }
            else yield return null;
        }
    }


}
