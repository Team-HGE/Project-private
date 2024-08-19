using UnityEngine;

public interface IInitializeByLoadedData
{
    public void InitializeByData();
}
[System.Serializable]
public class IInitializeByLoadedDataWraper
{
    public GameObject myObj;
    public IInitializeByLoadedData initializeByLoadedData;
    public IInitializeByLoadedDataWraper(IInitializeByLoadedData target) 
    {
        initializeByLoadedData = target;
        myObj = ((MonoBehaviour)target).gameObject;
    }

    public void InitializeByData()
    {
        myObj.GetComponent<IInitializeByLoadedData>().InitializeByData();
    }
}
