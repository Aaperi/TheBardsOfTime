﻿using UnityEngine;
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

    public bool paused, canvasOn,
        dialoguesPlaying;  //Used for the check whether an dialogue was playing before pausing.

    EventSystem eventSystem;
    DialogueScript dia;
    HPScript hp;

	// Use this for initialization
	void Start () {
        GetButtonReferences();
        eventSystem = FindObjectOfType<EventSystem>();
        dia = FindObjectOfType<DialogueScript>();
        hp = GameObject.FindGameObjectWithTag("Player").GetComponent<HPScript>();
        DisableAll();
        
	}
	
	// Update is called once per frame
	void Update () {
        if(AnyCanvasOn() || paused) 
            canvasOn = true;
         else 
            canvasOn = false;

        if (canvasOn || paused || dia.dialogueActive) {
            Time.timeScale = .000000001f;
        } else {
            Time.timeScale = 1f;
        }

        if(Input.GetKey(KeyCode.Escape) && !canvasOn){
            ShowPauseMenu();
        }

        if(Application.loadedLevelName == "menuScene") {
            ShowMainMenu();
        }

        Debug.Log(hp.HPSlider.value);
    }

    #region "Button clicks"
    public void PlayClick() {
        DisableAll();
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

        ShowMainMenu();

        SelectButton(play);
    }

    public void Options() {
        DisableAll();

        optionsCanvas.enabled = true;
        backButton.enabled = true;
    }

    public void OptionsBack() {
        DisableAll();

        ShowMainMenu();

        SelectButton(play);
    }

    public void ContinueButton() {
        paused = false;

        DisableAll();
    }

    public void QuitButton() {
        DisableAll();

        quitMenuCanvas.enabled = true;
        quitYesButton.enabled = true;
        quitNoButton.enabled = true;

        SelectButton(quitNo);
    }

    public void QuitYesButton() {
        DisableAll();

        ShowMainMenu();

        SelectButton(play);

        Application.LoadLevel(0);
    }

    public void QuitNoButton() {
        DisableAll();

        if(hp.iNeedUI && hp.HPSlider.value > 0) {
            ShowPauseMenu();
        }

        if (hp.iNeedUI && hp.HPSlider.value <= 0) {
            ShowGameOver();
        }

        SelectButton(cont);
    }

    public void RestartButton() {
        DisableAll();

        Application.LoadLevel(Application.loadedLevel);
    }

   /* public void AfterDeadQuit() {
        DisableAll();

        quitMenuCanvas.enabled = true;
        quitYesButton.enabled = true;
        quitNoButton.enabled = true;

        SelectButton(quitNo);
    }

    public void AfterDeadQuitNo() {
        DisableAll();

        ShowGameOver();

        SelectButton(restart);
    }*/

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
        backButton = back.GetComponent<Button>();
    }

    bool AnyCanvasOn() {
        if (pauseMenuCanvas.enabled || mainMenuCanvas.enabled || exitMenuCanvas.enabled || quitMenuCanvas.enabled
            || optionsCanvas.enabled || gameoverCanvas.enabled)
            return true;
        else return false;
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

    void ShowPauseMenu() {
        paused = true;

        pauseMenuCanvas.enabled = true;

        continueButton.enabled = true;
        quitButton.enabled = true;

        SelectButton(cont);
    }

    public void ShowGameOver() {
        DisableAll();

        gameoverCanvas.enabled = true;
        restartButton.enabled = true;
        quitToMenuButton.enabled = true;

        SelectButton(restart);
    }
}
