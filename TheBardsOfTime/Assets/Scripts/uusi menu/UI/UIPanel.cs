using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class UIPanel : MonoBehaviour {

	public Text text, diaText, actionGuide;
    public Image diaImage;
	public Button 
	continueButton, optionsButton, quitButton,
	backButton, saveButton, loadButton,
    yesButton, noButton;
	public Toggle CameraControlToggle, InvertToggle;
	public GameObject 
	uiPanelObject;

    [HideInInspector]
    public Text noteCount;

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

    private void Awake() {
        HideAll();
    }

    public void PauseChoice(string text, UnityAction continueEvent, UnityAction optionsEvent, UnityAction quitEvent, UnityAction saveEvent, UnityAction loadEvent){
		Debug.Log ("Pause");
		HideAll ();
		continueButton.onClick.RemoveAllListeners ();
		continueButton.onClick.AddListener (continueEvent);
		continueButton.onClick.AddListener (HideAll);

        saveButton.onClick.RemoveAllListeners();
        saveButton.onClick.AddListener(saveEvent);

        loadButton.onClick.RemoveAllListeners();
        loadButton.onClick.AddListener(loadEvent);
        loadButton.onClick.AddListener(HideAll);

		optionsButton.onClick.RemoveAllListeners ();
		optionsButton.onClick.AddListener (optionsEvent);

		quitButton.onClick.RemoveAllListeners ();
		quitButton.onClick.AddListener (quitEvent);

		this.text.text = text;
		continueButton.gameObject.SetActive (true);
        saveButton.gameObject.SetActive(true);
        loadButton.gameObject.SetActive(true);
		optionsButton.gameObject.SetActive (true);
		quitButton.gameObject.SetActive (true);
	}

	public void OptionsChoice(string text, UnityAction backEvent){
		Debug.Log ("Options");
		HideAll ();
		backButton.onClick.RemoveAllListeners ();
		backButton.onClick.AddListener (backEvent);

		this.text.text = text;
		backButton.gameObject.SetActive (true);
		CameraControlToggle.gameObject.SetActive (true);
		InvertToggle.gameObject.SetActive (true);
	}

    public void QuitChoice(string text, UnityAction yesEvent, UnityAction noEvent) {
		Debug.Log ("Quit");
        HideAll();
        yesButton.onClick.RemoveAllListeners();
        yesButton.onClick.AddListener(yesEvent);

        noButton.onClick.RemoveAllListeners();
        noButton.onClick.AddListener(noEvent);

        this.text.text = text;
        yesButton.gameObject.SetActive(true);
        noButton.gameObject.SetActive(true);
    }

	public void HideAll(){
		continueButton.gameObject.SetActive (false);
		optionsButton.gameObject.SetActive (false);
		quitButton.gameObject.SetActive (false);

		backButton.gameObject.SetActive (false);
        saveButton.gameObject.SetActive(false);
        loadButton.gameObject.SetActive(false);
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);

        CameraControlToggle.gameObject.SetActive (false);
		InvertToggle.gameObject.SetActive (false);

		this.text.text = "";
	}
}
