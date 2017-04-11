﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class MenuPanel : MonoBehaviour {

	public Text text;
	public Button 
	playButton, optionButton, exitButton;
	public GameObject 
		menuPanelObject;

    SoundScript ss;
    private static MenuPanel menuPanel;

	public static MenuPanel Instance(){
		if (!menuPanel) {
			menuPanel = FindObjectOfType (typeof(MenuPanel)) as MenuPanel;
			if (!menuPanel) {
				Debug.LogError ("There needs to be one active menuPanel");
			}
		}
		return
			menuPanel;
	}

    public void MenuChoice(string text, UnityAction playEvent, UnityAction optionEvent, UnityAction exitEvent){
		playButton.onClick.RemoveAllListeners ();
      //  playButton.onClick.AddListener(StopSong);
		playButton.onClick.AddListener (playEvent);
        playButton.onClick.AddListener (closePanel);

		optionButton.onClick.RemoveAllListeners ();
		optionButton.onClick.AddListener (optionEvent);
        optionButton.onClick.AddListener (closePanel);

		exitButton.onClick.RemoveAllListeners ();
		exitButton.onClick.AddListener (exitEvent);
        exitButton.onClick.AddListener (closePanel);

		this.text.text = text;
		playButton.gameObject.SetActive (true);
		optionButton.gameObject.SetActive (true);
		exitButton.gameObject.SetActive (true);
	}

	void closePanel(){
		menuPanelObject.SetActive (false);
	}

    /*void StopSong() {
        ss.StopSound("tbotti_karvalakki");
    }*/

}
