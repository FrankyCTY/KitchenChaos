using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnFlashingProgressBarUI : MonoBehaviour
{
    private const float ALMOST_BURN_PROGERESS_THRESHOLD = .5f;
    private const string IS_FLASHING = "isFlashing";
    
    [SerializeField] private StoveCounter stoveCounter;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        stoveCounter.HandleProgressChanged += StoveCounter_HandleProgressChanged;
        animator.SetBool(IS_FLASHING, false);
    }

    private void StoveCounter_HandleProgressChanged(object sender, IHasProgress.HandleProgressChangedEventArgs e)
    {
        animator.SetBool(IS_FLASHING, isAlmostBurn(e.progressNormalized));
    }

    
    private bool isAlmostBurn(float stoveProgressNormalized)
    {
        return stoveCounter.isFried() && stoveProgressNormalized >= ALMOST_BURN_PROGERESS_THRESHOLD;
    }
}
