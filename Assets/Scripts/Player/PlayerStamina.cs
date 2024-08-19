using UnityEngine;
using System.Collections;

public class RunEffect : MonoBehaviour
{
    //private PlayerStateMachine stateMachine;
    
    public float MaxStamina = 100f;
    public float CurrentStamina;
    public float DecreaseRate = 1f;
    public bool IsExhausted => CurrentStamina <= 5f;
    public bool CanRun => CurrentStamina >= 40f;

    public AudioSource audioSource; // 추가: AudioSource 컴포넌트
    public AudioClip recoveryClip; // 추가: 회복 시 재생할 오디오 클립
    private bool isRecovering = false; // 추가: 회복 중 상태를 추적

    private void Awake()
    {
        CurrentStamina = MaxStamina;
        //stateMachine = GetComponent<PlayerStateMachine>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // 추가: AudioSource 컴포넌트가 없을 경우 추가
        }


    }

    private void Update()
    {
        if (CurrentStamina <= 0 && !isRecovering)
        {
            StartRecovery(); // 추가: 스테미나가 0이 되고 회복 중이 아닌 경우 회복 시작
        }
    }


    public void ConsumeStamina(float DecreaseRate)
    {
        if (CurrentStamina > 0)
        {
            CurrentStamina -= DecreaseRate * Time.deltaTime;
            if (CurrentStamina < 5)
            {
                CurrentStamina = 0;
            }
        }
    }

    public void IncreaseStaminaIdle()
    {
        RecoverStamina(26f); 
    }
    public void IncreaseWalkIdle()
    {
        RecoverStamina(14f);
    }

    //private void RecoverStamina()
    //{
    //    float recoverRate = 0f;

    //    if (stateMachine.IsRuning)
    //    {
    //        return; 
    //    }

    //    if (stateMachine.Player.InputsData.MovementInput == Vector2.zero)
    //    {
    //        recoverRate = 22f; 
    //    }
    //    else
    //    {
    //        recoverRate = 11f;
    //    }

    //    CurrentStamina += recoverRate * Time.deltaTime;

    //    if (CurrentStamina > MaxStamina)
    //    {
    //        CurrentStamina = MaxStamina;
    //    }
    //    CheckStaminaRecovery();
    //}

    private void RecoverStamina(float recoverRate)
    {
        CurrentStamina += recoverRate * Time.deltaTime;

        if (CurrentStamina > MaxStamina)
        {
            CurrentStamina = MaxStamina;
        }
        CheckStaminaRecovery();
    }
    private void StartRecovery()
    {
        isRecovering = true; // 추가: 회복 중 상태로 설정
        audioSource.clip = recoveryClip; // 추가: 오디오 클립 설정
        audioSource.Play(); // 추가: 오디오 재생
    }
    private void CheckStaminaRecovery()
    {
        if (CurrentStamina >= 40 && isRecovering)
        {
            isRecovering = false; // 추가: 회복 중 상태 해제
            StartCoroutine(FadeOutAudio(0.3f)); // 추가: 오디오 서서히 줄이기 시작
        }
    }
    private IEnumerator FadeOutAudio(float duration)
    {
        float startVolume = audioSource.volume; // 추가: 시작 볼륨 저장

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / duration); // 추가: 볼륨을 서서히 줄임
            yield return null;
        }

        audioSource.Stop(); // 추가: 오디오 정지
        audioSource.volume = startVolume; // 추가: 볼륨을 초기화
    }
}