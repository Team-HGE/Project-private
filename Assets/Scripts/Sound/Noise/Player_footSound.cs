using UnityEngine;

public class FootstepAudio : MonoBehaviour
{
    public AudioClip footSlowClip;
    public AudioClip footFastClip;

    private AudioSource audioSource;
    private CharacterController characterController;
    private bool isRunning = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 플레이어가 움직이는지 확인
        if (characterController.isGrounded && characterController.velocity.magnitude > 0.1f)
        {
            // Shift 키를 눌러 달리는 상태인지 확인
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (!isRunning || audioSource.clip != footFastClip)
                {
                    // 달리기 시작
                    isRunning = true;
                    audioSource.clip = footFastClip;
                    audioSource.pitch = 0.6f; // 재생 속도 40% 느리게 설정
                    audioSource.loop = true; // 루프 설정
                    if (!audioSource.isPlaying)
                    {
                        audioSource.Play();
                    }
                }
            }
            else
            {
                if (isRunning || audioSource.clip != footSlowClip)
                {
                    // 걷기 시작
                    isRunning = false;
                    audioSource.clip = footSlowClip;
                    audioSource.pitch = 0.6f; // 재생 속도 40% 느리게 설정
                    audioSource.loop = true; // 루프 설정
                    if (!audioSource.isPlaying)
                    {
                        audioSource.Play();
                    }
                }
                else if (!audioSource.isPlaying)
                {
                    // 걷고 있지만 소리가 재생되지 않는 경우 (예: 처음 걷기 시작할 때)
                    audioSource.clip = footSlowClip;
                    audioSource.pitch = 0.6f; // 재생 속도 40% 느리게 설정
                    audioSource.loop = true; // 루프 설정
                    audioSource.Play();
                }
            }
        }
        else
        {
            // 플레이어가 멈추면 소리 정지
            audioSource.Stop();
        }
    }
}