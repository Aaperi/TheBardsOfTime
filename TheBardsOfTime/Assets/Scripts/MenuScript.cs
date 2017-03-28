using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    private static GameObject GameController;
    public static MenuScript Instance {
        get {
            if (GameController == null) {
                GameController = Instantiate(Resources.Load("Menu"), Vector3.zero, Quaternion.identity) as GameObject;
            }
            return GameController.GetComponent<MenuScript>();
        }
    }

    public Canvas
        mainMenuCanvas,
        exitMenuCanvas,
        pauseMenuCanvas,
        quitMenuCanvas,
        optionsCanvas,
        gameoverCanvas;

    public GameObject
        play, exit, options, // mainMenu
        back, invert, freeCamera, // options
        exitYes, exitNo, // exitGameMenu
        cont, quit, // pauseMenu
        quitYes, quitNo, // quitToMenu
        restart, quitToMenu, // gameoverMenu
        actionGuide; // is displayed when player is next to an item or npc

    private Button
        playButton, exitButton, optionsButton, // mainMenu
        backButton, // options
        exitYesButton, exitNoButton, // exitMenu
        continueButton, quitButton,  // pauseMenu
        quitYesButton, quitNoButton, // quitToMenu
        restartButton, quitToMenuButton; // gameoverMenu

    private Toggle
        invertTog, freeCam;

    public bool 
        paused, canvasOn,
        freeCamEnabled, invertEnabled,
        dialoguesPlaying;  //Used for the check whether an dialogue was playing before pausing.

    EventSystem eventSystem;
    DialogueScript dia;
    HPScript hp;
    Text aText;

    // Use this for initialization
    void Start()
    {
        init();
        GetButtonReferences();
        eventSystem = FindObjectOfType<EventSystem>();
        dia = FindObjectOfType<DialogueScript>();

        freeCamEnabled = GameManager.Instance.freeCamEnabled;
        invertEnabled = GameManager.Instance.invertEnabled;

        freeCam.isOn = freeCamEnabled;
        invertTog.isOn = invertEnabled;

        try {
            hp = GameObject.FindGameObjectWithTag("Player").GetComponent<HPScript>();
        }
        catch { }
        DisableAll();

        if (SceneManager.GetActiveScene().name == "menuScene") {
            ShowMainMenu();
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (AnyCanvasOn() || paused)
            canvasOn = true;
        else
            canvasOn = false;

        if (canvasOn || paused || dia.dialogueActive) {
            Time.timeScale = .000000001f;
        }
        else {
            Time.timeScale = 1f;
        }

        if (Input.GetKey(KeyCode.Escape) && !canvasOn) {
            ShowPauseMenu();
        }
    }

    void init()
    {
        actionGuide = GameObject.Find("actionGuide");
        aText = GameObject.Find("aText").GetComponent<Text>();
        actionGuide.SetActive(false);
    }

    void GetButtonReferences()
    {
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
        invertTog = invert.GetComponent<Toggle>();
        freeCam = freeCamera.GetComponent<Toggle>();
    }

    bool AnyCanvasOn()
    {
        if (pauseMenuCanvas.enabled || mainMenuCanvas.enabled || exitMenuCanvas.enabled || quitMenuCanvas.enabled
            || optionsCanvas.enabled || gameoverCanvas.enabled)
            return true;
        else return false;
    }

    void DisableAllCanvases()
    {
        mainMenuCanvas.enabled = false;
        exitMenuCanvas.enabled = false;
        pauseMenuCanvas.enabled = false;
        quitMenuCanvas.enabled = false;
        optionsCanvas.enabled = false;
        gameoverCanvas.enabled = false;
    }

    void DisableAllButtons()
    {
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

        // Options
        backButton.enabled = false;
        invertTog.enabled = false;
        freeCam.enabled = false;

    }

    void DisableAll()
    {
        DisableAllButtons();
        DisableAllCanvases();
    }

    void SelectButton(GameObject button)
    {
        eventSystem.SetSelectedGameObject(button);
    }

    void ShowMainMenu()
    {
        mainMenuCanvas.enabled = true;

        playButton.enabled = true;
        exitButton.enabled = true;
        optionsButton.enabled = true;

        SelectButton(play);
    }

    void ShowPauseMenu()
    {
        paused = true;

        pauseMenuCanvas.enabled = true;

        continueButton.enabled = true;
        quitButton.enabled = true;

        SelectButton(cont);
    }

    public void ShowGameOver()
    {
        DisableAll();

        gameoverCanvas.enabled = true;
        restartButton.enabled = true;
        quitToMenuButton.enabled = true;

        SelectButton(restart);
    }

    public void ShowGuide(string message)
    {
        actionGuide.SetActive(true);
        aText.text += message;
    }

    public void HideGuide()
    {
        aText.text = aText.text.Substring(0, 13);
        actionGuide.SetActive(false);
    }

    #region "Button clicks"
    public void PlayClick() {
        DisableAll();
        SceneManager.LoadScene(1); // should be in the build
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
        invertTog.enabled = true;
        freeCam.enabled = true;
        backButton.enabled = true;
    }

    public void OptionsBack() {
        DisableAll();

        if (SceneManager.GetActiveScene().name == "MainMenu") {
            ShowMainMenu();
        } else {
            ShowPauseMenu();
        }


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

        SceneManager.LoadScene(0);
    }

    public void QuitNoButton() {
        DisableAll();

        if (hp.iNeedUI && hp.HPSlider.value > 0) {
            ShowPauseMenu();
        }

        if (hp.iNeedUI && hp.HPSlider.value <= 0) {
            ShowGameOver();
        }

        SelectButton(cont);
    }

    public void RestartButton() {
        DisableAll();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void InvertToggleChanged() {
        invertEnabled = invertTog.isOn;
        Debug.Log(GameManager.Instance.invertEnabled);
    }

    public void FreeCamToggleChanged() {
        freeCamEnabled = freeCam.isOn;
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
}
