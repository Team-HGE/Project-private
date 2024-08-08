using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSystem : MonoBehaviour
{
    public Light thisLight;
    public MeshRenderer[] parentRenderer;
    private void Start()
    {
        thisLight ??= GetComponent<Light>();
        GameManager.Instance.lightManager.lobbyLights.Add(thisLight);
        if (parentRenderer != null)
        {
            foreach (var renderer in parentRenderer)
            {
                GameManager.Instance.lightManager.lobbyObjectRenderer.Add(renderer);
            }
        }
    }
}
