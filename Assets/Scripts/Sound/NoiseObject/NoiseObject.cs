using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseObject : InteractableObject, INoise
{
    [field: Header("Noise")]
    [field: SerializeField] public NoiseData NoiseData { get; set; }
    //[field: SerializeField] public float Duration { get; set; }
    // INoise
    [field: SerializeField] public float SumNoiseAmount { get; set; }
    [field: SerializeField] public float DecreaseSpeed { get; set; }
    [field: SerializeField] public float NoiseTransitionTime { get; set; }
    [field: SerializeField] public float CurNoiseAmount { get; set; } = 0f;

    public bool loop = false;
    private bool isUse;

    private void Awake()
    {
        if (NoiseData == null)
        {
            Debug.LogError("노이즈 데이터가 없습니다");
            return;
        }
        else
        {
            NoiseTransitionTime = NoiseData.transitionTime;

            if (NoisePool.Instance == null)
            {
                Debug.LogError($"NoiseObject - Awake - NoisePool 없음, {NoiseData.tag}");
            }
            else
            {
                NoisePool.Instance.noiseDatasList.Add(NoiseData);
                NoisePool.Instance.FindNoise();
            }
        }
    }

    private void Update()
    {
        if (isUse)
        {
            if (NoiseTransitionTime > 0)
            {
                CurNoiseAmount += NoiseData.volume;
                if (CurNoiseAmount >= SumNoiseAmount) CurNoiseAmount = SumNoiseAmount;

                CurNoiseAmount -= DecreaseSpeed * Time.deltaTime;
                if (CurNoiseAmount <= 0f) CurNoiseAmount = 0f;
            }             
        }
    }

    public override void ActivateInteraction()
    {
        if (isUse) GameManager.Instance.player.playerInteraction.SetActive(false);
        else GameManager.Instance.player.playerInteraction.SetActive(true);

        GameManager.Instance.player.interactableText.text = "작동시키기";
    }

    public override void Interact()
    {
        if (isUse) return;
        //Debug.Log("Interact()");
        isUse = true;

        PlayNoise(NoiseData.noises[0], NoiseData.tag, 0, 0, NoiseTransitionTime, 0, loop);

        StartCoroutine(TurnOff());
    }

    IEnumerator TurnOff()
    {
        yield return new WaitForSeconds(NoiseTransitionTime);

        isUse = false;
        CurNoiseAmount = 0f;
    }

    public void PlayNoise(AudioClip audioClip, string tag, float amount, float addVolume, float transitionTime, float pitch, bool loop)
    {
        SoundSource soundSource;
        soundSource = NoiseManager.Instance.PlayNoise(audioClip, tag, addVolume, transitionTime, pitch, loop);
        //return soundSource;
    }

    //public SoundSource PlayNoise(AudioClip[] audioClips, string tag, float amount, float addVolume, float transitionTime, float pitch)
    //{
    //    throw new System.NotImplementedException();
    //}
}
