using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    private const float ALMOST_BURN_PROGERESS_THRESHOLD = .5f;
    
    [SerializeField] private StoveCounter stoveCounter;
    
    private AudioSource audioSource;
    private float warningSoundTimer;
    private bool shouldPlayWarningSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        stoveCounter.HandleStateChanged += StoveCounter_HandleStateChanged;
        stoveCounter.HandleProgressChanged += StoveCounter_HandleProgressChanged;
    }

    private void StoveCounter_HandleProgressChanged(object sender, IHasProgress.HandleProgressChangedEventArgs e)
    {
        shouldPlayWarningSound = isAlmostBurn(e.progressNormalized);
    }

    private void StoveCounter_HandleStateChanged(object sender, StoveCounter.HandleStateChangedEventArgs e)
    {
        bool playSound = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;
        if (playSound)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
        }
    }

    private void Update()
    {
        if (shouldPlayWarningSound) Update_PlayWarningSoundEvery200Ms();
    }
    
    private Boolean isAlmostBurn(float stoveProgressNormalized)
    {
        return stoveCounter.isFried() && stoveProgressNormalized >= ALMOST_BURN_PROGERESS_THRESHOLD;
    }

    private void Update_PlayWarningSoundEvery200Ms()
    { 
        warningSoundTimer -= Time.deltaTime;
        Debug.Log(warningSoundTimer + " and " + Time.deltaTime);
        // Timer's time up
        if (warningSoundTimer <= 0f)
        {
            SoundManager.Instance.PlayWarningSound(stoveCounter.transform.position);
            // After playing, reset to .2f, so that when time goes by (x -= Time.deltaTime), after another 200ms
            // it will play again.
            warningSoundTimer = .2f;
        }
    }
}
