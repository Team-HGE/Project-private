public interface IInteractable
{
    bool isInteractable { get; set; }

    void Interact(); // 상호작용할 오브젝트의 실행 내용

    void ActivateInteraction(); // 메세지 출력
}