using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuActions : MonoBehaviour {

	private MenuPanel menuPanel;
	private DisplayManager displayManager;
    private GameManager gm;

	private UnityAction playAction;
	private UnityAction optionAction;
	private UnityAction exitAction;

	void Awake() {
		menuPanel = MenuPanel.Instance ();
		displayManager = DisplayManager.Instance ();
        gm = FindObjectOfType<GameManager>();

		playAction = new UnityAction (Play);
		optionAction = new UnityAction (Options);
		exitAction = new UnityAction (Exit);
	}

	public void Menu(){
		menuPanel.MenuChoice ("Testaa toimiiko napit \n", playAction, optionAction, exitAction);

	}

	public void Play(){
        StopSong();
        SceneManager.LoadScene ("uusiLevelStart");
	}

	public void Options(){
		SceneManager.LoadScene ("Options");
	}
		
	public void Exit(){
		Application.Quit ();
	}


    void StopSong() {
        SoundScript ss = FindObjectOfType<SoundScript>();
        ss.StopSound("tbotti_karvalakki");
    }
}
