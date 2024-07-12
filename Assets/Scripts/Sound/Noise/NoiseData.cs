using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class NoiseData
{
    public string tag;
    public GameObject prefab;
    public AudioClip[] noises;
    public float volume;
    public float transitionTime;

    public Transform box;
}

[Serializable]
public class NoiseDatasList
{    
    public List<NoiseData> noiseDatasList;    
}
