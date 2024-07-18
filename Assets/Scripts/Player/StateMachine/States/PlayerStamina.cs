using UnityEngine;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine.InputSystem;
using System.Collections;

public class RunEffect : MonoBehaviour
{
    private PlayerStateMachine stateMachine;
    
    public float MaxStamina = 100f;
    public float CurrentStamina;
    public float DecreaseRate = 18f;
    public bool IsExhausted => CurrentStamina <= 5f;
    public bool CanRun => CurrentStamina >= 40f;

    public AudioSource audioSource; // �߰�: AudioSource ������Ʈ
    public AudioClip recoveryClip; // �߰�: ȸ�� �� ����� ����� Ŭ��
    private bool isRecovering = false; // �߰�: ȸ�� �� ���¸� ����

    private void Awake()
    {
        CurrentStamina = MaxStamina;
        stateMachine = GetComponent<PlayerStateMachine>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // �߰�: AudioSource ������Ʈ�� ���� ��� �߰�
        }


    }

    private void Update()
    {
        if (CurrentStamina <= 0 && !isRecovering)
        {
            StartRecovery(); // �߰�: ���׹̳��� 0�� �ǰ� ȸ�� ���� �ƴ� ��� ȸ�� ����
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
        RecoverStamina(22f); 
    }
    public void IncreaseWalkIdle()
    {
        RecoverStamina(11f);
    }

    private void RecoverStamina()
    {
        float recoverRate = 0f;

        if (stateMachine.IsRuning)
        {
            return; 
        }

        if (stateMachine.Player.InputsData.MovementInput == Vector2.zero)
        {
            recoverRate = 22f; 
        }
        else
        {
            recoverRate = 11f;
        }

        CurrentStamina += recoverRate * Time.deltaTime;

        if (CurrentStamina > MaxStamina)
        {
            CurrentStamina = MaxStamina;
        }
        CheckStaminaRecovery();
    }

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
        isRecovering = true; // �߰�: ȸ�� �� ���·� ����
        audioSource.clip = recoveryClip; // �߰�: ����� Ŭ�� ����
        audioSource.Play(); // �߰�: ����� ���
    }
    private void CheckStaminaRecovery()
    {
        if (CurrentStamina >= 40 && isRecovering)
        {
            isRecovering = false; // �߰�: ȸ�� �� ���� ����
            StartCoroutine(FadeOutAudio(0.3f)); // �߰�: ����� ������ ���̱� ����
        }
    }
    private IEnumerator FadeOutAudio(float duration)
    {
        float startVolume = audioSource.volume; // �߰�: ���� ���� ����

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / duration); // �߰�: ������ ������ ����
            yield return null;
        }

        audioSource.Stop(); // �߰�: ����� ����
        audioSource.volume = startVolume; // �߰�: ������ �ʱ�ȭ
    }
}