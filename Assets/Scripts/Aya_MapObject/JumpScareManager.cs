using UnityEngine;
using UnityEngine.AI;
using System;
using System.Collections;
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
    public static JumpScareManager Instance;

    [Header("FlashLight")]
    public Light flashLight;

    [Header("Death")]
    public GameObject playerCanvas;
    public GameObject deathCanvas;
    public GameObject blackBG;

    [Header("MonsterControllers")]
    public NavMeshAgent[] monstersNavMeshAgent;

    [Header("MonsterType")]
    public MonstersJumpScare[] monstersJumpScare;


    public void PlayJumpScare(JumpScareType jumpScareType)
    {
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
