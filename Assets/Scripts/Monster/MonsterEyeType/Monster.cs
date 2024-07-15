using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    [field: Header("Reference")]
    [field: SerializeField] public MonsterSO Data { get; private set; }

    [field: Header("Animations")]
    [field: SerializeField] public MonsterAnimationData AnimationData { get; private set; }

    [field: Header("Noise")]
    [field: SerializeField] public NoiseData[] NoiseDatas { get; private set; }


    public CharacterController Controller { get; private set; }
    public ForceReceiver ForceReceiver { get; private set; }
    public Animator Animator { get; private set; }
    // Ai Nav
    public NavMeshAgent Agent { get; private set; }

    private MonsterStateMachine _stateMachine;

    // 행동 관리
    public bool IsBehavior {get; set;} = true;

    // INoise
    public float NoiseTransitionTime { get; set; }
    public float NoiseMin { get; set; }
    public float NoiseMax { get; set; }
    public float NoiseAmount { get; set; }
    public float DecreaseSpeed { get; set; }


    private void Awake()
    {
        AnimationData.Initialize();
        Animator = GetComponentInChildren<Animator>();
        Controller = GetComponent<CharacterController>();
        //Ai Nav
        Agent = GetComponent<NavMeshAgent>();
        ForceReceiver = GetComponent<ForceReceiver>();

        _stateMachine = new MonsterStateMachine(this);
    }

    private void Start()
    {
        _stateMachine.ChangeState(_stateMachine.PatrolState);
    }

    private void Update()
    {
        //_stateMachine.HandleInput();
        _stateMachine.Update();

        if (NoiseAmount > 0)
        {
            
        }
    }

    private void FixedUpdate()
    {
        _stateMachine.PhysicsUpdate();
    }
    
    // 대기 시간
    public void WaitForBehavior(float time)
    {
        StartCoroutine(ChangeBehavior(time));
    }

    public IEnumerator ChangeBehavior(float time)
    {
        // n초 대기
        yield return new WaitForSeconds(time);

        IsBehavior = !IsBehavior;
    }
}
