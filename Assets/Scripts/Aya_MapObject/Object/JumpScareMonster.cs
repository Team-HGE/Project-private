using System.Collections;
using UnityEngine;

public class JumpScareMonster : MonoBehaviour
{
    [SerializeField] GameObject monsterObject;
    [SerializeField] JumpScareType Type;
    [SerializeField] bool isTrigger;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isTrigger)
        {
            isTrigger = true;
            GameManager.Instance.jumpScareManager.PlayJumpScare(Type);
        }
    }

    private void Start()
    {
        StartCoroutine(FadeTime());
    }
    
    IEnumerator FadeTime()
    {
        float time = 0;
        while (time < 20)
        {
            time += Time.deltaTime;
            yield return null;
        }
        Destroy(monsterObject);
    }
}
