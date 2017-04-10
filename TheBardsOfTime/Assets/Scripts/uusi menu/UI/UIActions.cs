using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIActions : MonoBehaviour {

	private UIPanel uiPanel;
	private DisplayManager displayManager;

	private UnityAction continueAction;
	private UnityAction optionsAction;
	private UnityAction quitAction;
	private UnityAction backAction;
	private UnityAction cameraControlAction;
	private UnityAction invertAction;

	private GameManager game;

	void Awake(){
		uiPanel = UIPanel.Instance ();
		displayManager = DisplayManager.Instance ();
		game = FindObjectOfType<GameManager> ();

		continueAction = new UnityAction (UnPause);
		optionsAction = new UnityAction (Options);
		quitAction = new UnityAction (UnPause);
		backAction = new UnityAction (Back);
		cameraControlAction = new UnityAction (CameraControl);
		invertAction = new UnityAction (Invert);
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.O) && !uiPanel.continueButton.enabled)
			Pause ();
	}

	public void Pause(){
		uiPanel.PauseChoice ("Toimi nyt saatana \n", continueAction, optionsAction, quitAction);
	}

	public void UnPause(){
		
	}

	public void Options(){
		uiPanel.OptionsChoice ("Testaillaan nyt tätä \n", backAction, cameraControlAction, invertAction);
	}

	public void Quit(){
		
	}

	public void Back(){
		uiPanel.PauseChoice ("Toimi nyt saatana \n", continueAction, optionsAction, quitAction);
	}

	public void CameraControl(){
		if (uiPanel.CameraControlToggle.isOn)
			uiPanel.CameraControlToggle.isOn = false;
		else
			uiPanel.CameraControlToggle.isOn = true;

			CameraToggle ();
	}
	
	public void Invert(){
		if (uiPanel.InvertToggle.isOn)
			uiPanel.InvertToggle.isOn = false;
		else
			uiPanel.InvertToggle.isOn = true;

			InvertToggle ();
	}

	void CameraToggle(){
		game.freeCamEnabled = uiPanel.CameraControlToggle.isOn;
	}

	void InvertToggle(){
		game.invertEnabled = uiPanel.InvertToggle.isOn;
	}
}
