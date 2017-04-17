using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIActions : MonoBehaviour {

    private UIPanel uiPanel;
    private DisplayManager displayManager;
    private CC cc;

    private UnityAction 
        continueAction, optionsAction, quitAction, saveAction, loadAction,
        backAction, cameraControlAction, invertAction,
        yesAction, noAction;

    private GameManager game;
    private DialogueScript dia;

    private bool canvasOn, paused;

    void Awake() {
        uiPanel = UIPanel.Instance();
        displayManager = DisplayManager.Instance();
        game = FindObjectOfType<GameManager>();
        dia = FindObjectOfType<DialogueScript>();
        cc = FindObjectOfType<CC>();

        uiPanel.CameraControlToggle.isOn = game.freeCamEnabled;
        uiPanel.InvertToggle.isOn = game.invertEnabled;
		uiPanel.noteCount = game.noteCount;
		uiPanel.actionGuide.gameObject.SetActive (false);
        canvasOn = false;
        paused = false;

        continueAction = new UnityAction(UnPause);
        optionsAction = new UnityAction(Options);
        quitAction = new UnityAction(Quit);
        backAction = new UnityAction(Back);
        cameraControlAction = new UnityAction(CameraControl);
        invertAction = new UnityAction(Invert);
        saveAction = new UnityAction(SaveGame);
        loadAction = new UnityAction(LoadGame);
        yesAction = new UnityAction(Yes);
        noAction = new UnityAction(No);
    }

    void Update() {
        if (paused)
            canvasOn = true;
        else
            canvasOn = false;

		if (!dia.dialogueActive) {
			uiPanel.diaImage.enabled = false;
			uiPanel.diaText.text = "";
		}
        else
            uiPanel.diaImage.enabled = true;

        if (canvasOn || dia.dialogueActive) {
            Time.timeScale = .000000001f;
        } else {
            Time.timeScale = 1f;
        }

		Debug.Log (uiPanel.actionGuide.gameObject.activeSelf);
    }

    public void Pause() {
        paused = true;
        uiPanel.PauseChoice("PAUSE \n", continueAction, optionsAction, quitAction, saveAction, loadAction);
    }

    public void UnPause() {
        paused = false;
    }

    public void SaveGame() {
        game.Save();
    }

    public void LoadGame() {
        uiPanel.HideAll();
        paused = false;
        game.Load();
    }

    public void Options() {
        uiPanel.OptionsChoice("OPTIONS \n", backAction, cameraControlAction, invertAction);
    }

    public void Quit() {
        uiPanel.QuitChoice("HALUATKO POISTUA PELISTÄ? \n", yesAction, noAction);
    }

    public void Yes() {
        SceneManager.LoadScene(0);
    }

    public void No() {
        uiPanel.PauseChoice("PAUSE \n", continueAction, optionsAction, quitAction, saveAction, loadAction);
    }

    public void Back() {
        uiPanel.PauseChoice("PAUSE \n", continueAction, optionsAction, quitAction, saveAction, loadAction);
    }

    public void CameraControl() {
		if (!uiPanel.CameraControlToggle.isOn)
			uiPanel.CameraControlToggle.isOn = true;
		else
			uiPanel.CameraControlToggle.isOn = false;
		
        CameraToggle();
    }

    public void Invert() {
        if (!uiPanel.InvertToggle.isOn)
            uiPanel.InvertToggle.isOn = true;
        else
            uiPanel.InvertToggle.isOn = false;

        InvertToggle();
    }

    public void ShowGuide(string message) {
        uiPanel.actionGuide.gameObject.SetActive(true);
		uiPanel.actionGuide.text += message;
    }

    public void HideGuide() {
		uiPanel.actionGuide.text = uiPanel.actionGuide.text.Substring(0, 13);
		uiPanel.actionGuide.gameObject.SetActive(false);
    }

    void CameraToggle() {		
        game.freeCamEnabled = uiPanel.CameraControlToggle.isOn;
		Debug.Log (game.freeCamEnabled);
    }

    void InvertToggle() {
        game.invertEnabled = uiPanel.InvertToggle.isOn;
    }

    public void PauseGame() {
        Pause();
    }
}
