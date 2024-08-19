using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneManager : MonoBehaviour
{
    private static CutSceneManager _instance;

    public static CutSceneManager Instance
    {
        get
        {
            _instance ??= FindObjectOfType<CutSceneManager>();
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }



}
