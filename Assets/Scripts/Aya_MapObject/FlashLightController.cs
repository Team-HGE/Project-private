using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightController : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    public GameObject playerVC;
    public Light flashLight;
    public event Action toggleFlashLight;

    private void Start()
    {
        audioSource = playerVC.GetComponentInChildren<AudioSource>();
        toggleFlashLight += ToggleFlashLight;
    }
    public void ToggleFlashLight()
    {
        audioSource.Play();
        flashLight.enabled = !flashLight.enabled;
    }
}
