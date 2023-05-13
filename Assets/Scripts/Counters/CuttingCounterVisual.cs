using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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
        this.cuttingCounter.HandleCut += CuttingCounterHandleCut;
    }

    private void CuttingCounterHandleCut(object sender, EventArgs e)
    {
        this.animator.SetTrigger(CUT);
    }
}
