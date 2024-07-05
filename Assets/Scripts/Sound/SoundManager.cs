using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;//�ӽ� �ڵ� ���� ����***

    [SerializeField][Range(0f, 1f)] private float soundEffectVolume;
    [SerializeField][Range(0f, 1f)] private float soundEffectPitchVariance;
    [SerializeField][Range(0f, 1f)] private float musicVolume;

    private void Awake()
    {
        Instance = this;//�ӽ� �ڵ� ���� ����***
    }

    public void PlayNoise(AudioClip clip, string tag)
    {
        // ������Ʈ Ǯ�� SoundSource ����Ʈ�� ���� ���� ����
        GameObject obj = NoisePool.Instance.SpawnFromPool(tag);
        obj.SetActive(true);
        SoundSource soundSource = obj.GetComponent<SoundSource>();
        // effect ȿ������ ���� ��� ������ ���� ������Ʈ Ǯ������ ����
        // soundEffectPitchVariance �� ���带 �����ؼ� �Ѱ��� ���带 �پ��ϰ� ��� ����
        soundSource.Play(clip, Instance.soundEffectVolume, Instance.soundEffectPitchVariance);
    }
}