using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBaseState : IState
{
    protected PlayerStateMachine stateMachine;
    protected readonly PlayerGroundData groundData;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        groundData = stateMachine.Player.Data.GroundData;
    }

    public virtual void Enter()
    {
        // 이벤트 등록
        AddInputActionsCallbacks();
    }

    public virtual void Exit()
    {
        // 이벤트 해제
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
        // 점프 이벤트
        //input.playerActions.Jump.started += OnJumpStarted;//추가 구현사항***
    }

    protected virtual void RemoveInputActionsCallbacks()
    {
        PlayerController input = stateMachine.Player.Input;
        input.playerActions.Movement.canceled -= OnMovementCanceled;
        input.playerActions.Run.performed -= OnRunPerformed;
        input.playerActions.Run.canceled -= OnRunCanceled;
        input.playerActions.Crouch.performed -= OnCrouchPerformed;
        input.playerActions.Crouch.canceled -= OnCrouchCanceled;
        //input.playerActions.Jump.started -= OnJumpStarted;
    }

    public virtual void HandleInput()
    {
        ReadMovementInput();
    }

    public virtual void Update()
    {
        Move();
    }

    private void ReadMovementInput()
    {
        stateMachine.Player.InputsData.MovementInput = stateMachine.Player.Input.playerActions.Movement.ReadValue<Vector2>();
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

    protected virtual void OnRunPerformed(InputAction.CallbackContext context)
    {
        stateMachine.IsRuning = true;
    }

    protected virtual void OnRunCanceled(InputAction.CallbackContext context)
    {
        stateMachine.IsRuning = false;
    }

    protected virtual void OnCrouchPerformed(InputAction.CallbackContext context)
    {
        stateMachine.IsCrouch = true;
    }

    protected virtual void OnCrouchCanceled(InputAction.CallbackContext context)
    {
        stateMachine.IsCrouch = false;
    }








    // 자식 클래스에서 재정의 할 메서드
    public virtual void PhysicsUpdate()
    {
    }

    protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
    {
    }

    protected virtual void OnJumpStarted(InputAction.CallbackContext context)
    {
    }
}