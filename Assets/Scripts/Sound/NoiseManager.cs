using UnityEngine;

public class NoiseManager : SingletonManager<NoiseManager>
{       
    //public static NoiseManager Instance;//임시 코드 수정 예정***

    [SerializeField][Range(0f, 5f)] private float soundEffectVolume;
    [SerializeField][Range(0f, 5f)] private float soundEffectPitchVariance;

    protected override void Awake()
    {
        if (Instance != this)
        {
            Debug.Log("노이즈 매니저 이미 있음");
        }
    }

    //protected override void Awake()
    //{
    //    base.Awake();
    //}

    public SoundSource PlayNoise(AudioClip clip, string tag, float addVolume, float transitionTime, float pitch)
    {
        // 오브젝트 풀의 SoundSource 리스트로 사운드 파일 관리
        GameObject obj = NoisePool.Instance.SpawnFromPool(tag);
        obj.SetActive(true);
        SoundSource soundSource = obj.GetComponent<SoundSource>();
        // effect 효과음은 많고 몇개가 나올지 몰라서 오브젝트 풀링으로 관리
        // soundEffectPitchVariance 로 사운드를 조절해서 한가지 사운드를 다양하게 사용 가능
        soundSource.Play(clip, soundEffectVolume, soundEffectPitchVariance, addVolume, transitionTime, pitch);

        return soundSource;
    }
}