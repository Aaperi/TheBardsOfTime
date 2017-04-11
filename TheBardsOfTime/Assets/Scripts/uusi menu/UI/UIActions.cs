using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIActions : MonoBehaviour {

    private UIPanel uiPanel;
    private DisplayManager displayManager;

    private UnityAction 
        continueAction, optionsAction, quitAction, saveAction, loadAction,
        backAction, cameraControlAction, invertAction;

    private GameManager game;
    private DialogueScript dia;

    private bool canvasOn, paused = false;

    void Awake() {
        uiPanel = UIPanel.Instance();
        displayManager = DisplayManager.Instance();
        game = FindObjectOfType<GameManager>();
        dia = FindObjectOfType<DialogueScript>();

        continueAction = new UnityAction(UnPause);
        optionsAction = new UnityAction(Options);
        quitAction = new UnityAction(UnPause);
        backAction = new UnityAction(Back);
        cameraControlAction = new UnityAction(CameraControl);
        invertAction = new UnityAction(Invert);
        saveAction = new UnityAction(SaveGame);
        loadAction = new UnityAction(LoadGame);
    }

    void Update() {        
        if (paused || dia.dialogueActive) {
            Time.timeScale = .000000001f;
        } else {
            Time.timeScale = 1f;
        }

        if (!dia.dialogueActive)
            uiPanel.diaImage.enabled = false;
        else
            uiPanel.diaImage.enabled = true;

    }

    public void Pause() {
        paused = true;
        uiPanel.PauseChoice("Toimi nyt saatana \n", continueAction, optionsAction, quitAction, saveAction, loadAction);
    }

    public void UnPause() {
        paused = false;
    }

    public void SaveGame() {
        game.Save();
    }

    public void LoadGame() {
        game.Load();
    }

    public void Options() {
        uiPanel.OptionsChoice("Testaillaan nyt tätä \n", backAction, cameraControlAction, invertAction);
    }

    public void Quit() {

    }

    public void Back() {
        uiPanel.PauseChoice("Toimi nyt saatana \n", continueAction, optionsAction, quitAction, saveAction, loadAction);
    }

    public void CameraControl() {
        if (uiPanel.CameraControlToggle.isOn)
            uiPanel.CameraControlToggle.isOn = false;
        else
            uiPanel.CameraControlToggle.isOn = true;

        CameraToggle();
    }

    public void Invert() {
        if (uiPanel.InvertToggle.isOn)
            uiPanel.InvertToggle.isOn = false;
        else
            uiPanel.InvertToggle.isOn = true;

        InvertToggle();
    }

    public void ShowGuide(string message) {
        uiPanel.diaText.gameObject.SetActive(true);
        uiPanel.diaText.text = message;
    }

    public void HideGuide() {
        uiPanel.diaText.text = uiPanel.diaText.text.Substring(0, 13);
        uiPanel.diaText.gameObject.SetActive(false);
    }

    void CameraToggle() {
        game.freeCamEnabled = uiPanel.CameraControlToggle.isOn;
    }

    void InvertToggle() {
        game.invertEnabled = uiPanel.InvertToggle.isOn;
    }
}
