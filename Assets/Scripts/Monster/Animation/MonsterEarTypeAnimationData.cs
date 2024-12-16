using System;
using UnityEngine;
[Serializable]
public class MonsterEarTypeAnimationData
{
    [SerializeField] private string groundParameterName = "@Ground";
    [SerializeField] private string idleParameterName = "Idle";
    [SerializeField] private string patrolParameterName = "Patrol";
    [SerializeField] private string moveParameterName = "Move";
    [SerializeField] private string focusParameterName = "Focus";
    [SerializeField] private string comeBackParameterName = "ComeBack";
    [SerializeField] private string chaseParameterName = "Chase";
    [SerializeField] private string attackParameterName = "Attack";

    public int GroundParameterHash { get; private set; }
    public int IdleParameterHash { get; private set; }
    public int PatrolParameterHash { get; private set; }
    public int MoveParameterHash { get; private set; }
    public int FocusParameterHash { get; private set; }
    public int ComeBackParameterHash { get; private set; }
    public int ChaseParameterHash { get; private set; }
    public int AttackParameterHash { get; private set; }

    public void Initialize()
    {
        GroundParameterHash = Animator.StringToHash(groundParameterName);
        IdleParameterHash = Animator.StringToHash(idleParameterName);
        PatrolParameterHash = Animator.StringToHash(patrolParameterName);
        MoveParameterHash = Animator.StringToHash(moveParameterName);
        FocusParameterHash = Animator.StringToHash(focusParameterName);
        ComeBackParameterHash = Animator.StringToHash(comeBackParameterName);
        ChaseParameterHash = Animator.StringToHash(chaseParameterName);
        AttackParameterHash = Animator.StringToHash(attackParameterName);
    }
}
