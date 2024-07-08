using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;//임시 코드 수정 예정***

    [SerializeField][Range(0f, 1f)] private float soundEffectVolume;
    [SerializeField][Range(0f, 1f)] private float soundEffectPitchVariance;
    [SerializeField][Range(0f, 1f)] private float musicVolume;

    private void Awake()
    {
        Instance = this;//임시 코드 수정 예정***
    }

    public void PlayNoise(AudioClip clip, string tag)
    {
        // 오브젝트 풀의 SoundSource 리스트로 사운드 파일 관리
        GameObject obj = NoisePool.Instance.SpawnFromPool(tag);
        obj.SetActive(true);
        SoundSource soundSource = obj.GetComponent<SoundSource>();
        // effect 효과음은 많고 몇개가 나올지 몰라서 오브젝트 풀링으로 관리
        // soundEffectPitchVariance 로 사운드를 조절해서 한가지 사운드를 다양하게 사용 가능
        soundSource.Play(clip, Instance.soundEffectVolume, Instance.soundEffectPitchVariance);
    }
}