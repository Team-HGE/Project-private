using UnityEngine;

public class ElevatorTeleport : MonoBehaviour
{
    [SerializeField] Vector3 targetPos;
    [SerializeField] ElevatorObject elevatorObject;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Elevator"))
        {
            Vector3 elevatorPos = targetPos;
            elevatorPos.y = other.transform.localPosition.y;

            Vector3 playerPos = GameManager.Instance.player.transform.position - other.transform.position;

            other.transform.localPosition = elevatorPos;
            if (elevatorObject.isPlayerIn)
            {
                playerPos += other.transform.position;

                playerPos.y = GameManager.Instance.player.transform.position.y;

                CharacterController controller = GameManager.Instance.player.GetComponent<CharacterController>();
                if (controller != null)
                {
                    controller.enabled = false;
                    GameManager.Instance.player.transform.position = playerPos;
                    controller.enabled = true;
                }
            }
        }
    }
}
