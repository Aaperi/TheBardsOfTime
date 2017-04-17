using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class OptionsPanel : MonoBehaviour {

	public Text text;
	public Button backButton;
	public Toggle CameraControlToggle, InvertToggle;
	public GameObject optionsPanelObject;

	private static OptionsPanel optionsPanel;

	public static OptionsPanel Instance(){
		if (!optionsPanel) {
			optionsPanel = FindObjectOfType (typeof(OptionsPanel)) as OptionsPanel;
			if (!optionsPanel) {
				Debug.LogError ("There needs to be one active menuPanel");
			}
		}
		return
			optionsPanel;
	}

	public void OptionsChoice(string text, UnityAction backEvent){
		backButton.onClick.RemoveAllListeners ();
		backButton.onClick.AddListener (backEvent);
		backButton.onClick.AddListener (closePanel);

		this.text.text = text;
		backButton.gameObject.SetActive (true);
	}

	void closePanel(){
		optionsPanelObject.SetActive (false);
	}

}
