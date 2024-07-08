using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class Player : MonoBehaviour, INoise
{
    [field: Header("References")]
    [field: SerializeField] public PlayerSO Data { get; private set; }

    [field: Header("Noise")]
    [field: SerializeField] public NoiseDatas NoiseDatas { get; private set; }

    public PlayerController Input { get; private set; }
    public CharacterController Controller { get; private set; }
    public ForceReceiver ForceReceiver { get; private set; }
    public PlayerInputsData InputsData { get; private set; }

    private PlayerStateMachine stateMachine;

    // INoise
    public float NoiseTransitionTime { get; set; }
    public float NoiseMin { get; set; }
    public float NoiseMax { get; set; }
    public float NoiseAmount { get; set; }
    public float DecreaseSpeed { get; set; }

    private void Awake()
    {
        Input = GetComponent<PlayerController>();
        Controller = GetComponent<CharacterController>();
        ForceReceiver = GetComponent<ForceReceiver>();
        InputsData = GetComponent<PlayerInputsData>();

        stateMachine = new PlayerStateMachine(this);

        Debug.Log($"{NoiseDatas.noiseDatas.Count}");
        for (int i = 0; i < NoiseDatas.noiseDatas.Count; i++)
        {
            NoisePool.Instance.noiseDatasList.Add(NoiseDatas.noiseDatas[i]);
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        stateMachine.ChangeState(stateMachine.IdleState);
    }

    private void Update()
    {
        stateMachine.HandleInput();
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate();
    }

    public SoundSource PlayNoise(AudioClip[] audioClips, string tag)
    {
        int index = Random.Range(0, audioClips.Length);
        Debug.Log(index);

        SoundSource soundSource;

       soundSource = NoiseManager.Instance.PlayNoise(audioClips[index], tag);

        return soundSource;
    }

    //public void PlayNoise(AudioClip[] audioClips, string tag)
    //{
    //    int index = Random.Range(0, audioClips.Length);
    //    Debug.Log(index);

    //    NoiseManager.Instance.PlayNoise(audioClips[index], tag);
    //}

    //public void PlayNoise(float transitionTime, float min, float max, float speed)
    //{
    //    // 소음 반복
    //    if (transitionTime > 0)
    //    {

    //    }
    //    // 소음 한번 발생
    //    else
    //    {

    //    }
    //}  
}