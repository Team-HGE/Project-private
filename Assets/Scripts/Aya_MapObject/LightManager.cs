using Sirenix.OdinInspector;
using System;
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
public enum Floor
{   // 비트연산자   1 << 1 만큼 뒤로 밀기
    AFloor1F = 1 << 0, 
    AFloor2F = 1 << 1, 
    AFloor3F = 1 << 2, 
    AFloor4F = 1 << 3, 
    AFloor5F = 1 << 4,

    BFloor1F = 1 << 10, 
    BFloor2F = 1 << 11, 
    BFloor3F = 1 << 12, 
    BFloor4F = 1 << 13, 
    BFloor5F = 1 << 14, 

    AFloor = AFloor1F | AFloor2F | AFloor3F | AFloor4F | AFloor5F,
    BFloor = BFloor1F | BFloor2F | BFloor3F | BFloor4F | BFloor5F,

    Lobby = 1 << 31
}
[Serializable]
public class FloorElements
{
    public List<Light> lights = new List<Light>();
    public List<MeshRenderer> renderers = new List<MeshRenderer>();
}

[Serializable]
public class LightManager : MonoBehaviour
{
    [TitleGroup("LightManager", "MonoBehaviour", alignment: TitleAlignments.Centered, horizontalLine: true, boldTitle: true, indent: false)]

    [Title("Lights & MeshRenderer Dictionary")]
    [ShowInInspector, DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout)]
    public Dictionary<Floor, FloorElements> elementsForFloors = new Dictionary<Floor, FloorElements>();
    public void AddLightToFloor(Floor floor, Light light)
    {
        if (!elementsForFloors.ContainsKey(floor))
        {
            elementsForFloors[floor] = new FloorElements();
        }
        elementsForFloors[floor].lights.Add(light);
    }

    public void AddRendererToFloor(Floor floor, MeshRenderer renderer)
    {
        if (!elementsForFloors.ContainsKey(floor))
        {
            elementsForFloors[floor] = new FloorElements();
        }
        elementsForFloors[floor].renderers.Add(renderer);
    }

    public List<Light> GetLightsForFloor(Floor floor)
    {
        return elementsForFloors.ContainsKey(floor) ? elementsForFloors[floor].lights : new List<Light>();
    }

    public List<MeshRenderer> GetRenderersForFloor(Floor floor)
    {
        return elementsForFloors.ContainsKey(floor) ? elementsForFloors[floor].renderers : new List<MeshRenderer>();
    }

    
    [Title("Laver")]
    public List<Lever> levers = new List<Lever>();

    [Title("Lobby")]
    public List<Light> lobbyLights = new List<Light>();
    public List<MeshRenderer> lobbyObjectRenderer = new List<MeshRenderer>();

    [TabGroup("Light", "UseLights", SdfIconType.Palette, TextColor = "yellow")]
    [TabGroup("Light", "UseLights")][SerializeField] Material Use_Y_Lights;
    [TabGroup("Light", "UseLights")][SerializeField] Material Use_W_Lights;
    [TabGroup("Light", "UseLights")][SerializeField] Material Use_WY_Lights;
    [TabGroup("Light", "UseLights")][SerializeField] Material Use_Week_Lights;
    [TabGroup("Light", "UseLights")][SerializeField] Material Use_Bar_Lights;

    [TabGroup("Light", "NoneLights", SdfIconType.Palette, TextColor = "white")]
    [TabGroup("Light", "NoneLights")][SerializeField] Material None_Y_Lights;
    [TabGroup("Light", "NoneLights")][SerializeField] Material None_W_Lights;
    [TabGroup("Light", "NoneLights")][SerializeField] Material None_WY_Lights;
    [TabGroup("Light", "NoneLights")][SerializeField] Material None_Week_Lights;
    [TabGroup("Light", "NoneLights")][SerializeField] Material None_Bar_Lights;

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
        foreach (var lever in levers)
        {
            lever.OffNowFloorAllLight();
        }
    }

    public void OffListLight(List<Light> lights)
    {
        foreach (var light in lights)
        {
            light.enabled = false;
        }
    }

    public void OnListLight(List<Light> lights)
    {
        foreach (var light in lights)
        {
            light.enabled = true;
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

    private int _floorPowerStatus = int.MaxValue; // 32개에 칸이 전부 1

    public bool isFloorPowerOn(Floor targetFloor)
    {
        if ((_floorPowerStatus & (int)targetFloor) == (int)targetFloor)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetFloorPowerStatus(Floor targetFloor, bool set)
    {
        if (set)
        {
            _floorPowerStatus |= (int)targetFloor;
        }
        else
        {
            _floorPowerStatus &= ~(int)targetFloor;
        }
    }
}
