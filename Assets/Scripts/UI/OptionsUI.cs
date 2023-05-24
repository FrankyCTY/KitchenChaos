using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set;  }
    
    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI soundEffectsText;
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private Transform pressToRebindKey_trxm;

    private Action handleOptionUIClosed;

    private void Awake()
    {
        Instance = this;
        
        soundEffectsButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        
        musicButton.onClick.AddListener(() =>
        {
           BackgroundMusicManager.Instance.ChangeVolume(); 
           UpdateVisual();
        });
        
        closeButton.onClick.AddListener(() =>
        {
            Hide();
            this.handleOptionUIClosed();
        });
    }

    private void Start()
    {
        GameManager.Instance.OnGameResumed += GameManager_HandleGameResumed;
        
        UpdateVisual();

        HidePressToRebindKey();
        Hide();
    }

    private void GameManager_HandleGameResumed(object sender, EventArgs e)
    {
        Hide();
    }

    private void UpdateVisual()
    {
        soundEffectsText.text = "Sound Effects: " + Mathf.Round(SoundManager.Instance.GetVolume() * 10f);
        musicText.text = "Music: " + Mathf.Round(BackgroundMusicManager.Instance.GetVolume() * 10f);
    }

    public void Show(Action handleOptionUIClosed)
    {
        this.handleOptionUIClosed = handleOptionUIClosed;
        gameObject.SetActive(true);
        
        soundEffectsButton.Select();
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void ShowPressToRebindKey()
    {
        pressToRebindKey_trxm.gameObject.SetActive(true);
    }
    
    public void HidePressToRebindKey()
    {
        pressToRebindKey_trxm.gameObject.SetActive(false);
    }
}
