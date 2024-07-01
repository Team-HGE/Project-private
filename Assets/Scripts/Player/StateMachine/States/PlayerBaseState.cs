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

    // �Է� �̺�Ʈ ���
    protected virtual void AddInputActionsCallbacks()
    {
        PlayerController input = stateMachine.Player.Input;
        input.PlayerActions.Movement.canceled += OnMovementCanceled;
        input.PlayerActions.Run.performed += OnRunPerformed;
        input.PlayerActions.Run.canceled += OnRunCanceled;
        input.PlayerActions.Jump.started += OnJumpStarted;
    }

    // �Է� �̺�Ʈ ����
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
        // �̵�Ű �Է�
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

    








    // �ڽ� Ŭ�������� ������ �� �޼���
    public virtual void PhysicsUpdate()
    {
        // ��ӹ޴� ������ ������
    }

    protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
    {
        // ��ӹ޴� ������ ������
    }

    protected virtual void OnRunPerformed(InputAction.CallbackContext context)
    {
        // ��ӹ޴� ������ ������
    }

    protected virtual void OnRunCanceled(InputAction.CallbackContext context)
    {
        // ��ӹ޴� ������ ������
    }

    protected virtual void OnJumpStarted(InputAction.CallbackContext context)
    {
        // ��ӹ޴� ������ ������
    }

}