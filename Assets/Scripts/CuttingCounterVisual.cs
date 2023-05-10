using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{
    [SerializeField] private CuttingCounter cuttingCounter;
    
    private Animator animator;
    private const string CUT = "Cut";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        this.cuttingCounter.HandleCuttingTriggered += CuttingCounter_HandleCuttingTriggered;
    }

    private void CuttingCounter_HandleCuttingTriggered(object sender, EventArgs e)
    {
        this.animator.SetTrigger(CUT);
    }
}
