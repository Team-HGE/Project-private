using UnityEngine;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine.InputSystem;

public class PlayerStamina : MonoBehaviour
{
    private PlayerStateMachine stateMachine;
    
    public float MaxStamina = 100f;
    public float CurrentStamina;
    public float DecreaseRate = 18f;
    public bool IsExhausted => CurrentStamina <= 5f;
    public bool CanRun => CurrentStamina >= 40f;

    private void Awake()
    {
        CurrentStamina = MaxStamina;
        stateMachine = GetComponent<PlayerStateMachine>();

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
    }

    private void RecoverStamina(float recoverRate)
    {
        CurrentStamina += recoverRate * Time.deltaTime;

        if (CurrentStamina > MaxStamina)
        {
            CurrentStamina = MaxStamina;
        }
    }
}