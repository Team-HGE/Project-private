using System.Collections;
using UnityEngine;

public interface IScript
{
    public void Init(ScriptSO _script);
    public void Print();
}

public class ItemScript : MonoBehaviour, IScript
{
    public ScriptSO scriptSO;

    public void Init(ScriptSO _script)
    {
        scriptSO = _script;
    }

    public void Print()
    {
        throw new System.NotImplementedException();
    }

}