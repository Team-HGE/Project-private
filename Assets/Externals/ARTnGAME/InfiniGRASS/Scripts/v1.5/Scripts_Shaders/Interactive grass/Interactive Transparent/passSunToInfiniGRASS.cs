using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class passSunToInfiniGRASS : MonoBehaviour
{
    public Light sunLight;
    public Material grassMat;
    //public List<Material> grassMats;
    //public Infinigras
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        grassMat.SetColor("_LightColorA", sunLight.color * sunLight.intensity);
        grassMat.SetVector("_WorldSpaceLightPosA", sunLight.transform.forward);
    }
}
