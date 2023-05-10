using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private CuttingCounter cuttingCounter;
    [SerializeField] private Image barImage;

    private void Start()
    {
        cuttingCounter.HandleProgressChanged += CuttingCounter_HandleProgressChanged;

        barImage.fillAmount = 0f;
        Hide();
    }

    private void CuttingCounter_HandleProgressChanged(object sender, CuttingCounter.HandleProgressChangedEventArgs e)
    {
        barImage.fillAmount = e.progressNormalized;
        
        if (barImage.fillAmount is 0 or >= 1)
        {
            Hide();
            return;
        }

        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);       
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
