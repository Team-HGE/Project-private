using UnityEngine;
using UnityEngine.AI;
using System;
using System.Collections;
using Sirenix.OdinInspector;
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
    public float time;
}
public class JumpScareManager : MonoBehaviour
{
    [TitleGroup("JumpScareManager", "MonoBehaviour", alignment: TitleAlignments.Centered, horizontalLine: true, boldTitle: true, indent: false)]
    [Header("FlashLight")]
    public Light flashLight;

    [TabGroup("Tab", "Death", SdfIconType.EmojiDizzy, TextColor = "black")]
    [TabGroup("Tab", "Death")] public GameObject playerCanvas;
    [TabGroup("Tab", "Death")] public GameObject deathCanvas;
    [TabGroup("Tab", "Death")] public GameObject blackBG;

    [Title("MonsterControllers")]
    public NavMeshAgent[] monstersNavMeshAgent;

    [Title("MonsterType")]
    public MonstersJumpScare[] monstersJumpScare;


    public void PlayJumpScare(JumpScareType jumpScareType)
    {
        //AudioManager.Instance.StopAllClips();
        GameManager.Instance.playerDie = true;
        flashLight.enabled = false;
        GameManager.Instance.PlayerStateMachine.Player.PlayerControllOff();
        playerCanvas.SetActive(false);
        blackBG.SetActive(true);
        foreach (var mon in monstersJumpScare)
        {
            if (mon.jumpScareType == jumpScareType)
            {
                mon.gameObject.SetActive(true);
                StartCoroutine(OnDeathCanvas(mon.time, mon.gameObject));
                break;
            }
        }
    }
    IEnumerator OnDeathCanvas(float time, GameObject monsterObject)
    {
        yield return new WaitForSeconds(time);
        monsterObject.SetActive(false);
        playerCanvas.SetActive(true);
        deathCanvas.SetActive(true);
    }
}
