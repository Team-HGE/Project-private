using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour, INoise
{
    [field: Header("Reference")]
    [field: SerializeField] public MonsterSO Data { get; private set; }

    [field: Header("Animations")]
    [field: SerializeField] public MonsterAnimationData AnimationData { get; private set; }

    public CharacterController Controller { get; private set; }
    public ForceReceiver ForceReceiver { get; private set; }
    public Animator Animator { get; private set; }
    // Ai Nav
    public NavMeshAgent Agent { get; private set; }

    private MonsterStateMachine _stateMachine;

    // �ൿ ����
    public bool IsBehavior {get; set;} = true;

    // INoise
    public float NoiseTransitionTime { get; set; }
    public float NoiseMin { get; set; }
    public float NoiseMax { get; set; }
    public float NoiseAmount { get; set; }

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
        _stateMachine.HandleInput();
        _stateMachine.Update();
    }

    private void FixedUpdate()
    {
        _stateMachine.PhysicsUpdate();
    }
    
    // ��� �ð�
    public void WaitForBehavior(float time)
    {
        StartCoroutine(ChangeBehavior(time));
    }

    public IEnumerator ChangeBehavior(float time)
    {
        // n�� ���
        yield return new WaitForSeconds(time);

        IsBehavior = !IsBehavior;
    }

    public void MakeNoise(float second = 0)
    {
        // ���� �ݺ�
        if (second > 0)
        {

        }
        // ���� �ѹ� �߻�
        else 
        {
            
        }
    }

    public void PlayNoise()
    {
        throw new System.NotImplementedException();
    }
}
