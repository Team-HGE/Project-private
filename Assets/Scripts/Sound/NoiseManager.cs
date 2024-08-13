using UnityEngine;

public class NoiseManager : SingletonManager<NoiseManager>
{       
    //public static NoiseManager Instance;//�ӽ� �ڵ� ���� ����***

    [SerializeField][Range(0f, 5f)] private float soundEffectVolume;
    [SerializeField][Range(0f, 5f)] private float soundEffectPitchVariance;

    protected override void Awake()
    {
        if (Instance != this)
        {
            Debug.Log("������ �Ŵ��� �̹� ����");
        }
    }

    //protected override void Awake()
    //{
    //    base.Awake();
    //}

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
}