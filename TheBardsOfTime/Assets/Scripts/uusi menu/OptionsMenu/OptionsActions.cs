using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;

public class OptionsActions : MonoBehaviour {

	private OptionsPanel optionsPanel;
	private DisplayManager displayManager;

	private UnityAction backAction;

	private GameManager game;

	void Awake() {
		optionsPanel = OptionsPanel.Instance ();
		displayManager = DisplayManager.Instance ();
		game = FindObjectOfType<GameManager> ();

		backAction = new UnityAction (Back);
	}

	public void Menu(){
		optionsPanel.OptionsChoice ("OPTIONS \n", backAction);

	}

	public void CameraControl(){
		game.freeCamEnabled = optionsPanel.CameraControlToggle.isOn;
	}

	public void Invert(){
		game.invertEnabled = optionsPanel.InvertToggle.isOn;
	}

	public void Back(){
		SceneManager.LoadScene ("uusiMenu");
	}
}
