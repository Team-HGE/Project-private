using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseObject : InteractableObject, INoise
{
    [field: Header("Noise")]
    [field: SerializeField] public NoiseData NoiseData { get; set; }

    // INoise
    public float NoiseTransitionTime { get; set; }
    [field: SerializeField] public float CurNoiseAmount { get; set; }
    public float SumNoiseAmount { get; set; }
    [field: SerializeField] public float DecreaseSpeed { get; set; }

    public float decreaseDelay;
    public float addVolume;
    public float addPitch;

    public bool isLoop = false;
    public bool isAvailable = false;
    public string text;

    private bool isUse;
    private BoxCollider _collider;
    private WaitForSeconds _waitTransition;


    private void Awake()
    {
        if (NoiseData.tag == "")
        {
            Debug.LogError("노이즈 데이터가 없습니다");
            return;
        }
        else
        {
            //Debug.Log($"노이즈 데이터있음 , {NoiseData.tag}");

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


        if (!isAvailable)
        {
            if (gameObject.TryGetComponent<BoxCollider>(out _collider)) _collider.isTrigger = true;
            else Debug.LogError($"NoiseObject - Awake - 콜라이더 없음, {NoiseData.tag}");
        }
        else
        {
            if (gameObject.TryGetComponent<BoxCollider>(out _collider)) _collider.isTrigger = false;
            else Debug.LogError($"NoiseObject - Awake - 콜라이더 없음, {NoiseData.tag}");
        }

        _waitTransition = new WaitForSeconds(NoiseTransitionTime);
    }

    public override void ActivateInteraction()
    {
        if (isAvailable && !isUse) GameManager.Instance.player.playerInteraction.SetActive(true);
        else GameManager.Instance.player.playerInteraction.SetActive(false);

        if (text == "")
        {
            Debug.LogError($"{NoiseData.tag}, 텍스트 없음");
        }

        GameManager.Instance.player.interactableText.text = text;
    }

    public override void Interact()
    {
        if (isUse) return;
        //Debug.Log("Interact()");
        isUse = true;

        PlayNoise(NoiseData.noises[0], NoiseData.tag, 0, 0, NoiseTransitionTime, 0, isLoop);
        CurNoiseAmount += NoiseData.volume;
        StartCoroutine(TurnOff());
    }

    IEnumerator TurnOff()
    {
        yield return _waitTransition;

        isUse = false;
        CurNoiseAmount = 0f;
    }


    public void PlayNoise(AudioClip audioClip, string tag, float amount, float addVolume, float transitionTime, float pitch, bool isLoop)
    {
        SoundSource soundSource;
        soundSource = NoiseManager.Instance.PlayNoise(audioClip, tag, addVolume, transitionTime, pitch, isLoop);
    }

    //public SoundSource PlayNoise(AudioClip[] audioClips, string tag, float amount, float addVolume, float transitionTime, float pitch)
    //{
    //    throw new System.NotImplementedException();
    //}
}
