using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerInputs PlayerInputs { get; private set; }
    public PlayerInputs.PlayerActions PlayerActions { get; private set; }

    private void Awake()
    {
        PlayerInputs = new PlayerInputs();
        PlayerActions = PlayerInputs.Player;
    }

    private void OnEnable()
    {
        PlayerInputs.Enable();
    }

    private void OnDisable()
    {
        PlayerInputs.Disable();
    }
}