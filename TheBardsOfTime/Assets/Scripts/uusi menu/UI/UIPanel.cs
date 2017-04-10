using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class UIPanel : MonoBehaviour {

	public Text text;
	public Button 
	continueButton, optionsButton, quitButton;
	public GameObject 
	menuPanelObject;

	private static UIPanel uiPanel;

	public static UIPanel Instance(){
		if (!uiPanel) {
			uiPanel = FindObjectOfType (typeof(UIPanel)) as UIPanel;
			if (!uiPanel) {
				Debug.LogError ("There needs to be one active menuPanel");
			}
		}
		return
			uiPanel;
	}

	public void MenuChoice(string text, UnityAction playEvent, UnityAction optionEvent, UnityAction exitEvent){
		playButton.onClick.RemoveAllListeners ();
		playButton.onClick.AddListener (playEvent);
		playButton.onClick.AddListener (closePanel);

		optionsButton.onClick.RemoveAllListeners ();
		optionsButton.onClick.AddListener (optionEvent);
		optionsButton.onClick.AddListener (closePanel);

		exitButton.onClick.RemoveAllListeners ();
		exitButton.onClick.AddListener (exitEvent);
		exitButton.onClick.AddListener (closePanel);

		this.text.text = text;
		playButton.gameObject.SetActive (true);
		optionButton.gameObject.SetActive (true);
		exitButton.gameObject.SetActive (true);
	}

	void closePanel(){
		menuPanelObject.SetActive (false);
	}
}
