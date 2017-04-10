using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class UIPanel : MonoBehaviour {

	public Text text;
	public Button 
	continueButton, optionsButton, quitButton,
	backButton, CameraControlButton, InvertButton;
	public Toggle CameraControlToggle, InvertToggle;
	public GameObject 
	uiPanelObject;

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

	public void PauseChoice(string text, UnityAction continueEvent, UnityAction optionsEvent, UnityAction quitEvent){
		HideAll ();
		continueButton.onClick.RemoveAllListeners ();
		continueButton.onClick.AddListener (continueEvent);
		continueButton.onClick.AddListener (HideAll);

		optionsButton.onClick.RemoveAllListeners ();
		optionsButton.onClick.AddListener (optionsEvent);
		//optionsButton.onClick.AddListener ();

		quitButton.onClick.RemoveAllListeners ();
		quitButton.onClick.AddListener (quitEvent);
		//quitButton.onClick.AddListener ();

		this.text.text = text;
		continueButton.gameObject.SetActive (true);
		optionsButton.gameObject.SetActive (true);
		quitButton.gameObject.SetActive (true);
	}

	public void OptionsChoice(string text, UnityAction backEvent, UnityAction controlEvent, UnityAction invertEvent){
		HideAll ();
		backButton.onClick.RemoveAllListeners ();
		backButton.onClick.AddListener (backEvent);
		//backButton.onClick.AddListener (DisableButtons);

		CameraControlButton.onClick.RemoveAllListeners ();
		CameraControlButton.onClick.AddListener (controlEvent);

		InvertButton.onClick.RemoveAllListeners ();
		InvertButton.onClick.AddListener (invertEvent);

		this.text.text = text;
		backButton.gameObject.SetActive (true);
		CameraControlButton.gameObject.SetActive (true);
		InvertButton.gameObject.SetActive (true);
		CameraControlToggle.gameObject.SetActive (true);
		InvertToggle.gameObject.SetActive (true);
	}

	public void HideAll(){
		continueButton.gameObject.SetActive (false);
		optionsButton.gameObject.SetActive (false);
		quitButton.gameObject.SetActive (false);

		backButton.gameObject.SetActive (false);
		CameraControlButton.gameObject.SetActive (false);
		InvertButton.gameObject.SetActive (false);

		CameraControlToggle.gameObject.SetActive (false);
		InvertToggle.gameObject.SetActive (false);

		this.text.text = "";
	}
}
