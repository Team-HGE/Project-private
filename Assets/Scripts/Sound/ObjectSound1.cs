using UnityEngine;

public class ObjectSound1 : MonoBehaviour
{
    public AudioSource audioSource; // 재생할 소리 클립
    public float maxDistance = 30f; // 소리가 재생되는 최대 거리
    
    private Transform playerTransform;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어의 Transform 가져오기
    }

    void Update()
    {
        // 플레이어와 오브젝트 사이의 거리 계산
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // 플레이어가 오브젝트 근처에 있으면 소리 재생
        if (distanceToPlayer <= maxDistance)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play(); // 소리 재생
            }

             audioSource.volume = 0.6f - (distanceToPlayer / maxDistance);
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop(); // 플레이어가 멀어지면 소리 중지
            }
        }
    }
}