using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;

public class OptionsActions : MonoBehaviour {

	private OptionsPanel optionsPanel;
	private DisplayManager displayManager;

	private UnityAction backAction;
	private UnityAction cameraControlAction;
	private UnityAction invertAction;

	private GameManager game;

	void Awake() {
		optionsPanel = OptionsPanel.Instance ();
		displayManager = DisplayManager.Instance ();
		game = FindObjectOfType<GameManager> ();

		backAction = new UnityAction (Back);
		cameraControlAction = new UnityAction (CameraControl);
		invertAction = new UnityAction (Invert);
	}

	public void Menu(){
		optionsPanel.OptionsChoice ("Toimisko tää nyt \n", backAction, cameraControlAction, invertAction);

	}

	public void CameraControl(){
		if (optionsPanel.CameraControlToggle.isOn)
			optionsPanel.CameraControlToggle.isOn = false;
		else
			optionsPanel.CameraControlToggle.isOn = true;

		CameraToggle ();
	}

	public void Invert(){
		if (optionsPanel.InvertToggle.isOn)
			optionsPanel.InvertToggle.isOn = false;
		else
			optionsPanel.InvertToggle.isOn = true;

		InvertToggle ();
	}

	public void Back(){
		SceneManager.LoadScene ("uusiMenu");
	}

	void CameraToggle(){
		game.freeCamEnabled = optionsPanel.CameraControlToggle.isOn;
	}

	void InvertToggle(){
		game.invertEnabled = optionsPanel.InvertToggle.isOn;
	}
}
