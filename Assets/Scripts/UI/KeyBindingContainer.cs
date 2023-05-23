using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyBindingContainer : MonoBehaviour
{
    [SerializeField] private GameInput.Binding binding;
    [SerializeField] private TextMeshProUGUI bindingKeyText;
    [SerializeField] private Button bindingKeyButton;

    private void Awake()
    {
        bindingKeyButton.onClick.AddListener(() =>
        {
            RebindBinding(binding);
        });
    }

    private void Start()
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        bindingKeyText.text = GameInput.Instance.GetBindingText(binding);
    }
    
    private void RebindBinding(GameInput.Binding binding)
    {
        OptionsUI.Instance.ShowPressToRebindKey();
        GameInput.Instance.RebindBinding(binding, () =>
        {
            OptionsUI.Instance.HidePressToRebindKey();
            UpdateVisual();
        });
    }
}
