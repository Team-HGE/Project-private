using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseObject : InteractableObject, INoise
{
    [field: Header("Noise")]
    [field: SerializeField] public NoiseData NoiseData { get; set; }
    [field: SerializeField] public float NoiseTransitionTime { get; set; }
    [field: SerializeField] public float CurNoiseAmount { get; set; }
    [field: SerializeField] public float SumNoiseAmount { get; set; }
    [field: SerializeField] public float DecreaseSpeed { get; set; }

    [SerializeField] bool interactableOneTime;
    [SerializeField] bool isUse;

    //public AudioClip clip;
    //private AudioSource audioSource;

    //private void Start()
    //{
    //    audioSource = GetComponent<AudioSource>();
    //    NoiseTransitionTime = 15f;
    //}

    private void Update()
    {
        if (isUse)
        {
            CurNoiseAmount += 40f;
            if (CurNoiseAmount >= 100f) CurNoiseAmount = 100f;
        }
    }

    public override void ActivateInteraction()
    {       
        if (isUse) GameManager.Instance.player.playerInteraction.SetActive(false);
        else GameManager.Instance.player.playerInteraction.SetActive(true);

        GameManager.Instance.player.interactableText.text = "    [E] FirePlug Use";
    }

    public override void Interact()
    {
        if (isUse) return;
        //Debug.Log("Interact()");

        isUse = true;

        //if (clip == null) return;

       // audioSource.clip = clip;
        //audioSource.loop = true;
        //audioSource.Play();

        Invoke("TurnOff", NoiseTransitionTime);
    }

    private void TurnOff()
    {
        isUse = false;
        //audioSource.Stop();
        CurNoiseAmount = 0f;
    }

    public SoundSource PlayNoise(AudioClip[] audioClips, string tag, float amount, float addVolume, float transitionTime, float pitch)
    {
        throw new System.NotImplementedException();
    }
}
