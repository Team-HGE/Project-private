using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseBreakableObject : MonoBehaviour, INoise
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

    [field: Header("Object")]
    public GameObject noiseObject;
    public GameObject breackObject;

    [field: Header("State")]
    public bool isBreak = false;


    private bool _isErr = false;

    private BoxCollider _collider;
    private WaitForSeconds _waitDelay;


    private void Awake()
    {        
        if (NoiseData.tag == "")
        {
            Debug.LogError("노이즈 데이터가 없습니다");
            _isErr = true;
            return;
        }
        else
        {
            //Debug.Log($"노이즈 데이터있음 , {NoiseData.tag}");

            if (NoisePool.Instance == null)
            {
                Debug.LogError($"NoiseObject - Awake - NoisePool 없음, {NoiseData.tag}");
                _isErr = true;
            }
            else
            {
                NoisePool.Instance.noiseDatasList.Add(NoiseData);
                NoisePool.Instance.FindNoise();
            }
        }

        if (noiseObject == null || breackObject == null)
        {
            Debug.LogError($"오브젝트가 없습니다, {NoiseData.tag}");
            _isErr = true;
            return;
        }

        if (!gameObject.TryGetComponent<BoxCollider>(out _collider)) Debug.LogError($"NoiseObject - Awake - 콜라이더 없음, {NoiseData.tag}");

        _waitDelay = new WaitForSeconds(decreaseDelay);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isErr)
        {
            Debug.LogError($"NoiseBreakableObject - 에러");
            return;
        }

        if (isBreak || other.tag != "Player") return;

        // 깨짐
        Break();
        // 소음
        PlayNoise(NoiseData.noises[0], NoiseData.tag, addVolume, NoiseData.transitionTime, addPitch);

        if (decreaseDelay <= 0f)
        {
            Debug.LogError($"decreaseDelay 를 입력해주세요, {NoiseData.tag}");
            return;
        }

        // 소음 발생
        CurNoiseAmount += NoiseData.volume;        
        StartCoroutine(DecreaseNoise());
    }

    private void Break()
    {
        noiseObject.SetActive(false);
        breackObject.SetActive(true);

        //_collider.enabled = false;
        //_collider.isTrigger = false;
        isBreak = true;
    }

    public void PlayNoise(AudioClip audioClip, string tag, float addVolume, float transitionTime, float pitch)
    {
        SoundSource soundSource;
        soundSource = NoiseManager.Instance.PlayNoise(audioClip, tag, addVolume, transitionTime, pitch);
    }

    IEnumerator DecreaseNoise()
    {
        while (CurNoiseAmount > 0f)
        {
            yield return _waitDelay;

            CurNoiseAmount -= DecreaseSpeed;
            if (CurNoiseAmount <= 0) CurNoiseAmount = 0f;
        }
    }
}
