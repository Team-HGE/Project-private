using UnityEngine;
using Cinemachine;
using UnityEngine.AI;
using DG.Tweening;
using UnityEngine.UI;
public enum JumpScareType
{
    EyeTypeMonster,
    EarTypeMonster
}
public class JumpScareManager : MonoBehaviour
{
    public static JumpScareManager Instance;

    [Header("VC")]
    public Transform vcFollow;

    [Header("FlashLight")]
    public GameObject flashLight;

    [Header("Death")]
    public GameObject deathCanvas;

    public AudioClip[] jumpScareAudioClips;

    [Header("MonsterControllers")]
    [SerializeField] NavMeshAgent[] monstersNavMeshAgent;
    
}
