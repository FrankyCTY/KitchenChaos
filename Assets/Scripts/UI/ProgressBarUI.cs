using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image barImage;
    private IHasProgress hasProgressObj;

    private void Start()
    {
        hasProgressObj = hasProgressGameObject.GetComponent<IHasProgress>();
        if (hasProgressObj == null)
        {
            Debug.LogError("Game Object" + hasProgressGameObject + "does not have a component that implements IHasProgress!");
        }
        
        hasProgressObj.HandleProgressChanged += HasProgress_HandleProgressChanged;

        barImage.fillAmount = 0f;
        Hide();
    }

    private void HasProgress_HandleProgressChanged(object sender, IHasProgress.HandleProgressChangedEventArgs e)
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
