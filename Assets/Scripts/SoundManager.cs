using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClipRefsSO audioClipRefsSO;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_HandleRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFail += DeliveryManager_HandleRecipeFail;

        CuttingCounter.OnAnyCut += CuttingCounter_HandleAnyCut;

        Player.Instance.OnPickUp += Player_HandlePickUp;

        BaseCounter.OnAnyObjectPlacedHere += BaseCounter_HandlerAnyObjectPlacedHere;

        TrashCounter.OnAnyObjectTrashed += TrashCounter_HandleAnyObjectTrashed;
    }

    private void TrashCounter_HandleAnyObjectTrashed(object sender, EventArgs e)
    {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(audioClipRefsSO.trash, trashCounter.transform.position);
    }

    private void BaseCounter_HandlerAnyObjectPlacedHere(object sender, EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioClipRefsSO.objectDrop, baseCounter.transform.position);
    }

    private void Player_HandlePickUp(object sender, EventArgs e)
    {
        PlaySound(audioClipRefsSO.objectPickup, Player.Instance.transform.position);
    }

    private void CuttingCounter_HandleAnyCut(object sender, EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipRefsSO.chop,  cuttingCounter.transform.position);
    }

    private void DeliveryManager_HandleRecipeSuccess(object sender, EventArgs e)
    {
        PlaySound(audioClipRefsSO.deliverySuccess, DeliveryCounter.Instance.transform.position);
    }

    private void DeliveryManager_HandleRecipeFail(object sender, EventArgs e)
    {
        PlaySound(audioClipRefsSO.deliveryFail, DeliveryCounter.Instance.transform.position);
    }


    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipArray[UnityEngine.Random.Range(0, audioClipArray.Length)], position, volume);
    }
    
    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }

    public void PlayFootstepsSound(Vector3 position, float volume)
    {
        PlaySound(audioClipRefsSO.footstep, position, volume);
    }
}
