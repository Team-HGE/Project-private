using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HotelFloorScene_DataManager))]
public class InitializeByLoadedDataSetter : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        HotelFloorScene_DataManager manager = (HotelFloorScene_DataManager)target;
        if (GUILayout.Button("Set"))
        {
            manager.SetInitializeLoadData();
        }
    }
}
