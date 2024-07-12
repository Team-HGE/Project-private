using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EarTypeMonster : MonoBehaviour
{
    [field: Header("Reference")]
    [field: SerializeField] public MonsterSO Data { get; private set; }

    [field: Header("Animations")]
    [field: SerializeField] public MonsterEarTypeAnimationData AnimationData { get; private set; }

    public CharacterController Controller { get; private set; }
    public ForceReceiver ForceReceiver { get; private set; }
    public Animator Animator { get; private set; }
    // Ai Nav
    public NavMeshAgent Agent { get; private set; }

    private MonsterEarTypeStateMachine _stateMachine;

    // 행동 관리
    public bool IsBehavior { get; set; } = true;

    public LayerMask targetLayer;
    public Collider[] noiseMakers;

    private void Awake()
    {
        AnimationData.Initialize();
        Animator = GetComponentInChildren<Animator>();
        Controller = GetComponent<CharacterController>();
        //Ai Nav
        Agent = GetComponent<NavMeshAgent>();
        ForceReceiver = GetComponent<ForceReceiver>();

        _stateMachine = new MonsterEarTypeStateMachine(this);
    }

    private void Start()
    {
        _stateMachine.ChangeState(_stateMachine.PatrolState);
        //_stateMachine.ChangeState(_stateMachine.MoveState);

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
