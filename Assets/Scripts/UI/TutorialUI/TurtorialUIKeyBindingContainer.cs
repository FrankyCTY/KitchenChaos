using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUIKeyBindingContainer : MonoBehaviour
{
    [SerializeField] private GameInput.Binding binding;
    [SerializeField] private TextMeshProUGUI keyBindingText;

    private void Start()
    {
        UpdateVisual();
        GameInput.Instance.HandleBindingRebind += GameInput_HandleBindingRebind;
    }

    private void GameInput_HandleBindingRebind(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        keyBindingText.text = GameInput.Instance.GetBindingText(binding);
    }
}

