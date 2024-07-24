using UnityEngine;
public class EyeTypeMonsterJumpScare : MonoBehaviour
{
    [SerializeField] Transform monsterTransform;
    [SerializeField] float eyeHeight = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            JumpScareManager.Instance.OnJumpScare(monsterTransform, JumpScareType.EyeTypeMonster);
        }
    }
}
