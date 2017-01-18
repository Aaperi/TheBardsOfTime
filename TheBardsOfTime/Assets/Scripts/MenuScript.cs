using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class MenuScript : MonoBehaviour {

    public Canvas
        mainMenuCanvas,
        exitMenuCanvas,
        pauseMenuCanvas,
        quitMenuCanvas,
        optionsCanvas,
        gameoverCanvas;

    public GameObject
    play, exit, options, // mainMenu
    back, // options
    exitYes, exitNo, // exitGameMenu
    cont, quit, // pauseMenu
    quitYes, quitNo, // quitToMenu
    restart, quitToMenu; // gameoverMenu

    private Button
        playButton, exitButton, optionsButton, // mainMenu
        backButton, // options
        exitYesButton, exitNoButton, // exitMenu
        continueButton, quitButton,  // pauseMenu
        quitYesButton, quitNoButton, // quitToMenu
        restartButton, quitToMenuButton; // gameoverMenu

    public bool paused, canvasOn, firstLaunch;

    EventSystem eventSystem;

	// Use this for initialization
	void Start () {
        firstLaunch = true;
        GetButtonReferences();
        eventSystem = FindObjectOfType<EventSystem>();
        DisableAll();

        if (firstLaunch) {
          //  ShowMainMenu(); buildissa pitää olla
        }
        else {
            DisableAll();
        }
        
	}
	
	// Update is called once per frame
	void Update () {
        if(AnyCanvasOn() || paused) 
            canvasOn = true;
         else 
            canvasOn = false;

        if (canvasOn || paused) {
            Time.timeScale = .000000001f;
        } else {
            Time.timeScale = 1f;
        }

        if(Input.GetKey(KeyCode.Escape) && !canvasOn){
            ShowPauseMenu();
        }
            
	}

    #region "Button clicks"
    public void PlayClick() {
        DisableAll();
        firstLaunch = false;
        Application.LoadLevel(1); // should be in the build
    }

    public void ExitClick() {
        DisableAll();

        mainMenuCanvas.enabled = true;

        exitMenuCanvas.enabled = true;
        exitYesButton.enabled = true;
        exitNoButton.enabled = true;

        SelectButton(exitNo);
    }

    public void ExitYes() {
        Application.Quit(); // should be in the build
    }

    public void ExitNo() {
        DisableAll();

        mainMenuCanvas.enabled = true;
        playButton.enabled = true;
        exitButton.enabled = true;
        optionsButton.enabled = true;

        SelectButton(play);
    }

    public void Options() {
        DisableAll();

        optionsCanvas.enabled = true;
        backButton.enabled = true;
    }

    public void OptionsBack() {
        DisableAll();

        mainMenuCanvas.enabled = true;
        playButton.enabled = true;
        exitButton.enabled = true;
        optionsButton.enabled = true;

        SelectButton(play);
    }

    public void ContinueButton() {
        paused = false;

        DisableAll();
    }

    public void QuitButton() {
        DisableAll();

        pauseMenuCanvas.enabled = false;
        quitMenuCanvas.enabled = true;

        quitYesButton.enabled = true;
        quitNoButton.enabled = true;

        SelectButton(quitNo);
    }

    public void QuitYesButton() {
        DisableAll();

        mainMenuCanvas.enabled = true;
        playButton.enabled = true;
        exitButton.enabled = true;
        optionsButton.enabled = true;

        SelectButton(play);

        Application.LoadLevel(0);
    }

    public void QuitNoButton() {
        DisableAll();

        pauseMenuCanvas.enabled = true;
        quitButton.enabled = true;
        continueButton.enabled = true;

        SelectButton(cont);
    }

    public void RestartButton() {
        DisableAll();

        Application.LoadLevel(Application.loadedLevel);
    }

    public void AfterDeadQuit() {
        DisableAll();

        quitMenuCanvas.enabled = true;
        quitYesButton.enabled = true;
        quitNoButton.enabled = true;

        SelectButton(quitNo);
    }

    public void AfterDeadQuitNo() {
        DisableAll();

        gameoverCanvas.enabled = true;

        restartButton.enabled = true;
        quitToMenuButton.enabled = true;

        SelectButton(restart);
    }

    #endregion "Button Clicks"

    void GetButtonReferences() {
        playButton = play.GetComponent<Button>();
        exitButton = exit.GetComponent<Button>();
        exitYesButton = exitYes.GetComponent<Button>();
        exitNoButton = exitNo.GetComponent<Button>();
        optionsButton = options.GetComponent<Button>();
        continueButton = cont.GetComponent<Button>();
        quitButton = quit.GetComponent<Button>();
        quitYesButton = quitYes.GetComponent<Button>();
        quitNoButton = quitNo.GetComponent<Button>();
        restartButton = restart.GetComponent<Button>();
        quitToMenuButton = quitToMenu.GetComponent<Button>();
    }

    bool AnyCanvasOn() {
        if (pauseMenuCanvas.enabled || mainMenuCanvas.enabled || exitMenuCanvas.enabled || quitMenuCanvas.enabled || optionsCanvas.enabled || gameoverCanvas.enabled)
            return true;
        else return false;
    }

    void ShowPauseMenu() {
        paused = true;

        pauseMenuCanvas.enabled = true;

        continueButton.enabled = true;
        quitButton.enabled = true;

        SelectButton(cont);
    }

    void DisableAllCanvases() {
        mainMenuCanvas.enabled = false;
        exitMenuCanvas.enabled = false;
        pauseMenuCanvas.enabled = false;
        quitMenuCanvas.enabled = false;
        optionsCanvas.enabled = false;
        gameoverCanvas.enabled = false;
    }

    void DisableAllButtons() {
        // MainMenu
        playButton.enabled = false;
        exitButton.enabled = false;

        // ExitMenu
        exitYesButton.enabled = false;
        exitNoButton.enabled = false;

        // PauseMenu
        quitButton.enabled = false;
        continueButton.enabled = false;

        // QuitToMenu
        quitNoButton.enabled = false;
        quitYesButton.enabled = false;

        // GameOverMenu
        restartButton.enabled = false;
        quitToMenuButton.enabled = false;
    }

    void DisableAll() {
        DisableAllButtons();
        DisableAllCanvases();
    }

    void SelectButton(GameObject button) {
        eventSystem.SetSelectedGameObject(button);
    }

    void ShowMainMenu() {
        mainMenuCanvas.enabled = true;

        playButton.enabled = true;
        exitButton.enabled = true;
        optionsButton.enabled = true;

        SelectButton(play);
    }

    public void ShowGameOver() {
        DisableAll();

        gameoverCanvas.enabled = true;
        restartButton.enabled = true;
        quitToMenuButton.enabled = true;

        SelectButton(restart);
    }
}
