public interface IInteractable
{
    bool isInteractable { get; set; }

    bool oneTime { get; set; }

    void Interact(); // ��ȣ�ۿ��� ������Ʈ�� ���� ����

    void ActivateInteraction(); // �޼��� ���
}