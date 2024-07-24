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
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    [Header("VC")]
    public GameObject jumpScareObject;
    public Transform vcFollow;
    public CinemachineVirtualCamera monsterVC;
    public CinemachineVirtualCamera mainCamera;
    [SerializeField] Vector3 firstTake;
    [SerializeField] Vector3 secondTake;

    [Header("FlashLight")]
    public GameObject flashLight;

    [Header("Death")]
    public GameObject blackImage;

    [Header("Audio")]
    [SerializeField] AudioSource jumpScareAudioSources;
    public AudioClip[] jumpScareAudioClips;

    [Header("MonsterControllers")]
    [SerializeField] NavMeshAgent[] monstersNavMeshAgent;
    public void OnJumpScare(Transform target, JumpScareType jumpScareType)
    {
        jumpScareObject.transform.SetParent(target);
        jumpScareObject.transform.localPosition = Vector3.zero;
        jumpScareObject.transform.localRotation = new Quaternion(0,0,0,0);
        monsterVC.LookAt = target;
        jumpScareObject.SetActive(true);
        mainCamera.Priority = 0;
        monsterVC.Priority = 10;
        flashLight.SetActive(false);
        vcFollow.DOShakeRotation(1.2f, 15, 10);
        vcFollow.DOLocalMove(firstTake, 1f).onComplete += () =>
        {

            vcFollow.DOLocalMove(secondTake, 0.2f).onComplete += () => 
            {
                GameManager.Instance.fadeManager.FadeImmediately();
            };
        };
        OffMonsterController();
        
        switch (jumpScareType)
        {
            case JumpScareType.EyeTypeMonster:
                jumpScareAudioSources.clip = jumpScareAudioClips[0];
                jumpScareAudioSources.Play();
                break;
            case JumpScareType.EarTypeMonster:
                break;
        }
        GameManager.Instance.PlayerStateMachine.Player.PlayerControllOnOff();
    }

    void OffMonsterController()
    {
        foreach (var controller in monstersNavMeshAgent)
        {
            controller.enabled = false;
        }
    }
}