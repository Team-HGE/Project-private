using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LightName
{
    Use_Y_Lights,
    Use_W_Lights,
    Use_WY_Lights,
    Use_Week_Lights,
    Use_Bar_Lights,

    Unknown,

    None_Y_Lights,
    None_W_Lights,
    None_WY_Lights,
    None_Week_Lights,
    None_Bar_Lights
}
public class LightManager : MonoBehaviour
{
    [Header("Laver")]
    public List<Laver> lavers = new List<Laver>();

    [Header("Lobby")]
    public List<Light> lobbyLights = new List<Light>();
    public List<MeshRenderer> lobbyObjectRenderer = new List<MeshRenderer>();

    [Header("UseLights")]
    [SerializeField] Material Use_Y_Lights;
    [SerializeField] Material Use_W_Lights;
    [SerializeField] Material Use_WY_Lights;
    [SerializeField] Material Use_Week_Lights;
    [SerializeField] Material Use_Bar_Lights;

    [Header("noneLights")]
    [SerializeField] Material None_Y_Lights;
    [SerializeField] Material None_W_Lights;
    [SerializeField] Material None_WY_Lights;
    [SerializeField] Material None_Week_Lights;
    [SerializeField] Material None_Bar_Lights;

    private Dictionary<string, LightName> materailLightName = new Dictionary<string, LightName>()
    {
        { "Use_Bar_Lights", LightName.Use_Bar_Lights},
        { "Use_Y_Lights", LightName.Use_Y_Lights},
        { "Use_WY_Lights", LightName.Use_WY_Lights},
        { "Use_W_Lights", LightName.Use_W_Lights},
        { "Use_Week_Lights", LightName.Use_Week_Lights},

        { "None_Y_Lights", LightName.None_Y_Lights},
        { "None_W_Lights", LightName.None_W_Lights},
        { "None_WY_Lights", LightName.None_WY_Lights},
        { "None_Week_Lights", LightName.None_Week_Lights},
        { "None_Bar_Lights", LightName.None_Bar_Lights }
    };
    public void OffLaversAllLight()
    {
        foreach (var laver in lavers)
        {
            laver.OffNowFloorAllLight();
        }
    }

    public void OffListLight(List<Light> lights)
    {
        foreach (var light in lights)
        {
            light.enabled = false;
        }
    }
    public void OffChangeMaterial(MeshRenderer[] meshRenderers)
    {
        foreach (var renderer in meshRenderers)
        {
            if (renderer == null)
            {
                continue;
            }
            Material[] newMaterial = renderer.materials;
            for (int i = 0; i < newMaterial.Length; i++)
            {
                string materialName = newMaterial[i].name.Replace(" (Instance)", "");
                LightName newName = GetMaterialType(materialName);
                switch (newName)
                {
                    case LightName.Use_Bar_Lights:
                        newMaterial[i] = None_Bar_Lights;
                        break;
                    case LightName.Use_Y_Lights:
                        newMaterial[i] = None_Y_Lights;
                        break;
                    case LightName.Use_WY_Lights:
                        newMaterial[i] = None_WY_Lights;
                        break;
                    case LightName.Use_W_Lights:
                        newMaterial[i] = None_W_Lights;
                        break;
                    case LightName.Use_Week_Lights:
                        newMaterial[i] = None_Week_Lights;
                        break;
                    default: break;
                }
                renderer.materials = newMaterial;
            }
        }
    }
    public void OffChangeMaterial(List<MeshRenderer> meshRenderers)
    {
        foreach (var renderer in meshRenderers)
        {
            if (renderer == null)
            {
                continue;
            }
            Material[] newMaterial = renderer.materials;
            for (int i = 0; i < newMaterial.Length; i++)
            {
                string materialName = newMaterial[i].name.Replace(" (Instance)", "");
                LightName newName = GetMaterialType(materialName);
                switch (newName)
                {
                    case LightName.Use_Bar_Lights:
                        newMaterial[i] = None_Bar_Lights;
                        break;
                    case LightName.Use_Y_Lights:
                        newMaterial[i] = None_Y_Lights;
                        break;
                    case LightName.Use_WY_Lights:
                        newMaterial[i] = None_WY_Lights;
                        break;
                    case LightName.Use_W_Lights:
                        newMaterial[i] = None_W_Lights;
                        break;
                    case LightName.Use_Week_Lights:
                        newMaterial[i] = None_Week_Lights;
                        break;
                    default: break;
                }
                renderer.materials = newMaterial;
            }
        }
    }
    public void OnChangeMaterial(MeshRenderer[] meshRenderers)
    {
        foreach (var renderer in meshRenderers)
        {
            if (renderer == null)
            {
                continue;
            }
            Material[] newMaterial = renderer.materials;
            for (int i = 0; i < newMaterial.Length; i++)
            {
                string materialName = newMaterial[i].name.Replace(" (Instance)", "");
                LightName newName = GetMaterialType(materialName);
                switch (newName)
                {
                    case LightName.None_Bar_Lights:
                        newMaterial[i] = Use_Bar_Lights;
                        break;
                    case LightName.None_Y_Lights:
                        newMaterial[i] = Use_Y_Lights;
                        break;
                    case LightName.None_WY_Lights:
                        newMaterial[i] = Use_WY_Lights;
                        break;
                    case LightName.None_W_Lights:
                        newMaterial[i] = Use_W_Lights;
                        break;
                    case LightName.None_Week_Lights:
                        newMaterial[i] = Use_Week_Lights;
                        break;
                    default: break;
                }
                renderer.materials = newMaterial;
            }
        }
    }
    public void OnChangeMaterial(List<MeshRenderer> meshRenderers)
    {
        foreach (var renderer in meshRenderers)
        {
            if (renderer == null)
            {
                continue;
            }
            Material[] newMaterial = renderer.materials;
            for (int i = 0; i < newMaterial.Length; i++)
            {
                string materialName = newMaterial[i].name.Replace(" (Instance)", "");
                LightName newName = GetMaterialType(materialName);
                switch (newName)
                {
                    case LightName.None_Bar_Lights:
                        newMaterial[i] = Use_Bar_Lights;
                        break;
                    case LightName.None_Y_Lights:
                        newMaterial[i] = Use_Y_Lights;
                        break;
                    case LightName.None_WY_Lights:
                        newMaterial[i] = Use_WY_Lights;
                        break;
                    case LightName.None_W_Lights:
                        newMaterial[i] = Use_W_Lights;
                        break;
                    case LightName.None_Week_Lights:
                        newMaterial[i] = Use_Week_Lights;
                        break;
                    default: break;
                }
                renderer.materials = newMaterial;
            }
        }
    }
    LightName GetMaterialType(string materialName)
    {
        if (materailLightName.TryGetValue(materialName, out LightName materialType))
        {
            return materialType;
        }
        return LightName.Unknown;
    }
}
