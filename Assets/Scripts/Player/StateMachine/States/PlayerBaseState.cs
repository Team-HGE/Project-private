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
        AddInputActionsCallbacks();
    }

    public virtual void Exit()
    {
        RemoveInputActionsCallbacks();
    }

    // 입력 이벤트 등록
    protected virtual void AddInputActionsCallbacks()
    {
        PlayerController input = stateMachine.Player.Input;
        input.PlayerActions.Movement.canceled += OnMovementCanceled;
        input.PlayerActions.Run.performed += OnRunPerformed;
        input.PlayerActions.Run.canceled += OnRunCanceled;
        input.PlayerActions.Jump.started += OnJumpStarted;
    }

    // 입력 이벤트 해지
    protected virtual void RemoveInputActionsCallbacks()
    {
        PlayerController input = stateMachine.Player.Input;
        input.PlayerActions.Movement.canceled -= OnMovementCanceled;
        input.PlayerActions.Run.performed -= OnRunPerformed;
        input.PlayerActions.Run.canceled -= OnRunCanceled;
        input.PlayerActions.Jump.started -= OnJumpStarted;
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
        // 이동키 입력
        stateMachine.Player.InputsData.MovementInput = stateMachine.Player.Input.PlayerActions.Movement.ReadValue<Vector2>();
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

    








    // 자식 클래스에서 재정의 할 메서드
    public virtual void PhysicsUpdate()
    {
        // 상속받는 곳에서 재정의
    }

    protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
    {
        // 상속받는 곳에서 재정의
    }

    protected virtual void OnRunPerformed(InputAction.CallbackContext context)
    {
        // 상속받는 곳에서 재정의
    }

    protected virtual void OnRunCanceled(InputAction.CallbackContext context)
    {
        // 상속받는 곳에서 재정의
    }

    protected virtual void OnJumpStarted(InputAction.CallbackContext context)
    {
        // 상속받는 곳에서 재정의
    }

}