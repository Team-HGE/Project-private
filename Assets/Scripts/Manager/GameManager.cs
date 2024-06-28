using UnityEngine;

public class GameManager : SingletonManager<GameManager>
{
    [SerializeField] PlayerInteractable player;

    protected override void Awake()
    {
        base.Awake();
    }
}