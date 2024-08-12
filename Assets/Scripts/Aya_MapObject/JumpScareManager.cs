using UnityEngine;
using Cinemachine;
using UnityEngine.AI;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
public enum JumpScareType
{
    GroupTypeMonster,
    EyeTypeMonster,
    EarTypeMonster
}
[Serializable]
public class MonstersJumpScare
{
    public JumpScareType jumpScareType;
    public GameObject gameObject;
}
public class JumpScareManager : MonoBehaviour
{
    public static JumpScareManager Instance;

    [Header("FlashLight")]
    public GameObject flashLight;

    [Header("Death")]
    public GameObject deathCanvas;

    [Header("MonsterControllers")]
    public NavMeshAgent[] monstersNavMeshAgent;

    [Header("MonsterType")]
    public MonstersJumpScare[] monstersJumpScare;

    public void PlayJumpScare(JumpScareType jumpScareType)
    {
        monstersJumpScare[(int)jumpScareType].gameObject.SetActive(true);
    }
}
