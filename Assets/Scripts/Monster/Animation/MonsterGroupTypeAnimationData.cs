using System;
using UnityEngine;
[Serializable]
public class MonsterGroupTypeAnimationData
{
    [SerializeField] private string groundParameterName = "@Ground";
    [SerializeField] private string chaseParameterName = "Chase";
    [SerializeField] private string attackParameterName = "Attack";

    public int GroundParameterHash { get; private set; }
    public int ChaseParameterHash { get; private set; }
    public int AttackParameterHash { get; private set; }

    public void Initialize()
    {
        GroundParameterHash = Animator.StringToHash(groundParameterName);
        ChaseParameterHash = Animator.StringToHash(chaseParameterName);
        AttackParameterHash = Animator.StringToHash(attackParameterName);
    }
}
