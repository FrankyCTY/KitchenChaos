using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnWarningUI : MonoBehaviour
{
    private const float ALMOST_BURN_PROGERESS_THRESHOLD = .5f;
    
    [SerializeField] private StoveCounter stoveCounter;

    private void Start()
    {
        stoveCounter.HandleProgressChanged += StoveCounter_HandleProgressChanged;
        Hide();
    }

    private void StoveCounter_HandleProgressChanged(object sender, IHasProgress.HandleProgressChangedEventArgs e)
    {
        if (isAlmostBurn(e.progressNormalized))
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    
    private void Hide()
    {
        gameObject.SetActive(false);
    }
    
    private bool isAlmostBurn(float stoveProgressNormalized)
    {
        return stoveCounter.isFried() && stoveProgressNormalized >= ALMOST_BURN_PROGERESS_THRESHOLD;
    }
}
