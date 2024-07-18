using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class growSnowSM : MonoBehaviour
{
    public bool enableGUI = false;
    public float snowCoverage = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    void OnGUI()
    {
        if (enableGUI)
        {
            snowCoverage = GUI.HorizontalSlider(new Rect(Screen.width / 2 - 100, Screen.height - 230, 300, 30), snowCoverage, 0, 2);
        }
    }
    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalFloat("_SnowCoverage", snowCoverage);
    }
}
