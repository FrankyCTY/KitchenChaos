using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button openOptionMenuButton;

    private void Awake()
    {
        resumeButton.onClick.AddListener(() =>
        {
            GameManager.Instance.ToggleGamePause();
        });
        
        mainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        
        openOptionMenuButton.onClick.AddListener(() =>
        {
            Hide();
            // Ensure Game Pause UI is shown when user clicks the close button from the Options UI
            OptionsUI.Instance.Show(Show);
        });
    }

    private void Start()
    {
        GameManager.Instance.OnGamePaused += Instance_OnGamePaused;
        GameManager.Instance.OnGameResumed += Instance_OnGameResumed;

        Hide();
    }

    private void Instance_OnGameResumed(object sender, EventArgs e)
    {
        Hide();
    }

    private void Instance_OnGamePaused(object sender, EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
        
        // Select (hover) by default on pause UI show up
        resumeButton.Select();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
