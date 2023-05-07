using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private ClearCounter clearCounter;
    [SerializeField] private GameObject visualGameObject;

    private void Start()
    {
        Player.Instance.HandleSelectedCounterChanged += Player_HandleSelectedCounterChanged;
    }

    private void Player_HandleSelectedCounterChanged(object sender, Player.HandleSelectedCounterChangedEventArgs e)
    {
        if (e.selectedCounter == this.clearCounter)
        {
            this.Show();
        }
        else
        {
            this.Hide();
        }
    }

    private void Show()
    {
        this.visualGameObject.SetActive(true);
    }

    private void Hide()
    {
        this.visualGameObject.SetActive(false);
    }
}