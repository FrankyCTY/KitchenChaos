using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour
{
    [SerializeField] private ContainerCounter containerCounter;
    
    private Animator animator;
    private const string OPEN_CLOSE = "OpenClose";

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        this.containerCounter.HandlePlayerGrabbedObject += ContainerCounter_HandlePlayerGrabbedObject;
    }

    private void ContainerCounter_HandlePlayerGrabbedObject(object sender, EventArgs e)
    {
        this.animator.SetTrigger(OPEN_CLOSE);
    }
}
