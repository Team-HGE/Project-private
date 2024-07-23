using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class B_Scene_DataManager : MonoBehaviour
{
    private static B_Scene_DataManager _instance;
    public static B_Scene_DataManager Instance {  get { return _instance; } }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }
}
