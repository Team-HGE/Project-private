using System;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBaseState : IState
{
    protected PlayerStateMachine stateMachine;
    protected readonly PlayerGroundData groundData;

    private SoundSource curStepSource;
    private SoundSource curBreathSource;
    private int curWalkBreathIndex = 0;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        groundData = stateMachine.Player.Data.GroundData;
    }

    public virtual void Enter()
    {
        AddInputActionsCallbacks();
    }

    public virtual void Exit()
    {
        RemoveInputActionsCallbacks();
    }

    protected virtual void AddInputActionsCallbacks()
    {
        PlayerController input = stateMachine.Player.Input;
        input.playerActions.Movement.canceled += OnMovementCanceled;
        input.playerActions.Run.performed += OnRunPerformed;
        input.playerActions.Run.canceled += OnRunCanceled;
        input.playerActions.Crouch.performed += OnCrouchPerformed;
        input.playerActions.Crouch.canceled += OnCrouchCanceled;
        input.playerActions.Interaction.started += OnInterationStared;
        input.playerActions.Interaction.performed += OnInterationPerformed;
        input.playerActions.Interaction.canceled += OnInterationCanceled;
        input.playerActions.Flash.started += OnFlashStarted;
    }

    protected virtual void RemoveInputActionsCallbacks()
    {
        PlayerController input = stateMachine.Player.Input;
        input.playerActions.Movement.canceled -= OnMovementCanceled;
        input.playerActions.Run.performed -= OnRunPerformed;
        input.playerActions.Run.canceled -= OnRunCanceled;
        input.playerActions.Crouch.performed -= OnCrouchPerformed;
        input.playerActions.Crouch.canceled -= OnCrouchCanceled;
        input.playerActions.Interaction.started -= OnInterationStared;
        input.playerActions.Interaction.performed -= OnInterationPerformed;
        input.playerActions.Interaction.canceled -= OnInterationCanceled;
        input.playerActions.Flash.started -= OnFlashStarted;
    }

    public virtual void HandleInput()
    {
        // 플레이어 정지 - 이동 입력
        if (!stateMachine.Player.IsPlayerControll) return;

        ReadMovementInput();
    }

    public virtual void Update()
    {
        // 플레이어 정지 - 이동, 숨소리
        if (!stateMachine.Player.IsPlayerControll) return;

        Move();
        BreathNoise();
    }

    private void ReadMovementInput()
    {
        stateMachine.Player.InputsData.MovementInput = stateMachine.Player.Input.playerActions.Movement.ReadValue<Vector2>();

        if (stateMachine.Player.InputsData.MovementInput != Vector2.zero) StepNoise();
    }

    private void Move()
    {
        Vector3 movementDirection = GetMovementDirection();
        Move(movementDirection);        
    }

    private Vector3 GetMovementDirection()
    {
        Vector2 movementInput = stateMachine.Player.InputsData.MovementInput;
        Vector3 forward = stateMachine.Player.transform.forward;
        Vector3 right = stateMachine.Player.transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        return forward * movementInput.y + right * movementInput.x;
    }

    private void Move(Vector3 direction)
    {
        float movementSpeed = GetMovementSpeed();

        stateMachine.Player.Controller.Move
            (
                ((direction * movementSpeed) + stateMachine.Player.ForceReceiver.Movement) * Time.deltaTime
            );
    }

    private float GetMovementSpeed()
    {
        float movementSpeed = stateMachine.MovementSpeed * stateMachine.MovementSpeedModifier;
        return movementSpeed;
    }

    private void StepNoise()
    {
        NoiseData curStepData;

        if (curStepSource == null)
        {
            for (int i = 0; i < stateMachine.Player.NoiseDatasList.noiseDatasList.Count; i++)
            {
                if (stateMachine.IsRunning)
                {
                    if (stateMachine.Player.NoiseDatasList.noiseDatasList[i].tag == "RunStepNoise")
                    {
                        curStepData = stateMachine.Player.NoiseDatasList.noiseDatasList[i];
                        curStepSource = stateMachine.Player.PlayNoise(curStepData.noises, curStepData.tag, curStepData.volume, 0.5f, curStepData.transitionTime, 0f);
                        break;
                    }
                }
                else if (stateMachine.IsCrouch)
                {
                    if (stateMachine.Player.NoiseDatasList.noiseDatasList[i].tag == "WalkStepNoise")
                    {
                        curStepData = stateMachine.Player.NoiseDatasList.noiseDatasList[i];
                        curStepSource = stateMachine.Player.PlayNoise(curStepData.noises, curStepData.tag, curStepData.volume - 3f, -0.75f, curStepData.transitionTime + 0.5f, 0f);
                        break;
                    }
                }
                else 
                {
                    if (stateMachine.Player.NoiseDatasList.noiseDatasList[i].tag == "WalkStepNoise")
                    {
                        curStepData = stateMachine.Player.NoiseDatasList.noiseDatasList[i];
                        curStepSource = stateMachine.Player.PlayNoise(curStepData.noises, curStepData.tag, curStepData.volume, -0.5f, curStepData.transitionTime, 0f);
                        break;
                    }
                }                
            }
        }
        else
        {
            if (!curStepSource.gameObject.activeSelf) curStepSource = null;
        }
    }

    private void BreathNoise()
    {
        string walkBreathTag = "WalkBreathNoise";

        if (!stateMachine.PressShift)
        {
            NoiseData curBreathData;

            if (curBreathSource == null)
            {
                for (int i = 0; i < stateMachine.Player.NoiseDatasList.noiseDatasList.Count; i++)
                {
                    if (stateMachine.Player.NoiseDatasList.noiseDatasList[i].tag == walkBreathTag)
                    {
                        curBreathData = stateMachine.Player.NoiseDatasList.noiseDatasList[i];
                        curBreathSource = stateMachine.Player.PlayNoise(curBreathData.noises[curWalkBreathIndex], curBreathData.tag, curBreathData.volume, 0.1f, curBreathData.transitionTime, 0f);
                        curWalkBreathIndex++;
                        if (curWalkBreathIndex == 4) curWalkBreathIndex = 0;
                        break;
                    }
                }
            }
            else
            {
                if (!curBreathSource.gameObject.activeSelf) curBreathSource = null;
            }
        }
        else
        {
            NoiseData curBreathData;

            if (curBreathSource == null)
            {
                for (int i = 0; i < stateMachine.Player.NoiseDatasList.noiseDatasList.Count; i++)
                {
                    if (stateMachine.Player.NoiseDatasList.noiseDatasList[i].tag == walkBreathTag)
                    {
                        curBreathData = stateMachine.Player.NoiseDatasList.noiseDatasList[i];
                        curBreathSource = stateMachine.Player.PlayNoise(curBreathData.noises[curWalkBreathIndex], curBreathData.tag, curBreathData.volume + 3f, 0.5f, curBreathData.transitionTime - 0.5f, 0f);
                        curWalkBreathIndex++;
                        if (curWalkBreathIndex == 4) curWalkBreathIndex = 0;
                        break;
                    }
                }
            }
            else
            {
                if (!curBreathSource.gameObject.activeSelf) curBreathSource = null;
            }
        }
    }


    protected virtual void OnRunPerformed(InputAction.CallbackContext context)
    {
        stateMachine.PressShift = true;
    }

    protected virtual void OnRunCanceled(InputAction.CallbackContext context)
    {
        stateMachine.PressShift = false;
    }

    protected virtual void OnCrouchPerformed(InputAction.CallbackContext context)
    {
        stateMachine.PressCtrl = true;
    }

    protected virtual void OnCrouchCanceled(InputAction.CallbackContext context)
    {
        stateMachine.PressCtrl = false;
    }

    protected virtual void OnInterationStared(InputAction.CallbackContext context)
    {
        stateMachine.IsInteraction = true;
    } 

    protected virtual void OnInterationPerformed(InputAction.CallbackContext context)
    {
        stateMachine.IsInteraction = true;
    }

    protected virtual void OnInterationCanceled(InputAction.CallbackContext context)
    {
        stateMachine.IsInteraction = false;
    }

    // F키 상호작용 Flash
    protected virtual void OnFlashStarted(InputAction.CallbackContext context)
    {
        Debug.Log("F키 입력, 후레쉬 OnOff");
        stateMachine.Player.flashLightController.ToggleFlashLight();
    }

    // 자식 클래스에서 재정의 할 메서드
    public virtual void PhysicsUpdate()
    {
    }

    protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
    {
    }
}