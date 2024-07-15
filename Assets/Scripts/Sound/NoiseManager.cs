using UnityEngine;

public class NoiseManager : MonoBehaviour
{       
    public static NoiseManager Instance;//�ӽ� �ڵ� ���� ����***

    [SerializeField][Range(0f, 5f)] private float soundEffectVolume;
    [SerializeField][Range(0f, 5f)] private float soundEffectPitchVariance;
    //[SerializeField][Range(0f, 5f)] private float musicVolume;

    private void Awake()
    {
        //Debug.Log($"NoiseManager - Awake");

        Instance = this;//�ӽ� �ڵ� ���� ����***
    }

    public SoundSource PlayNoise(AudioClip clip, string tag, float addVolume, float transitionTime, float pitch)
    {
        // ������Ʈ Ǯ�� SoundSource ����Ʈ�� ���� ���� ����
        GameObject obj = NoisePool.Instance.SpawnFromPool(tag);
        obj.SetActive(true);
        SoundSource soundSource = obj.GetComponent<SoundSource>();
        // effect ȿ������ ���� ��� ������ ���� ������Ʈ Ǯ������ ����
        // soundEffectPitchVariance �� ���带 �����ؼ� �Ѱ��� ���带 �پ��ϰ� ��� ����
        soundSource.Play(clip, soundEffectVolume, soundEffectPitchVariance, addVolume, transitionTime, pitch);

        return soundSource;
    }


    //public void PlayNoise(AudioClip clip, string tag)
    //{
    //    // ������Ʈ Ǯ�� SoundSource ����Ʈ�� ���� ���� ����
    //    GameObject obj = NoisePool.Instance.SpawnFromPool(tag);
    //    obj.SetActive(true);
    //    SoundSource soundSource = obj.GetComponent<SoundSource>();
    //    // effect ȿ������ ���� ��� ������ ���� ������Ʈ Ǯ������ ����
    //    // soundEffectPitchVariance �� ���带 �����ؼ� �Ѱ��� ���带 �پ��ϰ� ��� ����
    //    soundSource.Play(clip, Instance.soundEffectVolume, Instance.soundEffectPitchVariance);
    //}
}