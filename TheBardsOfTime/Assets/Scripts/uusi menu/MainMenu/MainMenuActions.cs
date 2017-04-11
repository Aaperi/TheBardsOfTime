using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuActions : MonoBehaviour {

	private MenuPanel menuPanel;
	private DisplayManager displayManager;
    private GameManager gm;
    private SoundScript ss;

	private UnityAction playAction;
	private UnityAction optionAction;
	private UnityAction exitAction;

	void Awake() {
		menuPanel = MenuPanel.Instance ();
		displayManager = DisplayManager.Instance ();
        gm = FindObjectOfType<GameManager>();
        ss = FindObjectOfType<SoundScript>();

		playAction = new UnityAction (Play);
		optionAction = new UnityAction (Options);
		exitAction = new UnityAction (Exit);
	}

	public void Menu(){
        menuPanel.MenuChoice ("Testaa toimiiko napit \n", playAction, optionAction, exitAction);

	}

	public void Play(){
        ss.StopSound("tbotti_karvalakki");
        SceneManager.LoadScene ("uusiLevelStart");
	}

	public void Options(){
		SceneManager.LoadScene ("Options");
	}
		
	public void Exit(){
		Application.Quit ();
	}

}
