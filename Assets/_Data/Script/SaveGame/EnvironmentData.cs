using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class EnvironmentData
{
    public List<Transform> pickedUpItems;
    public List<string> constructedName;
    public List<float> constructedpos;
    public List<float> constructedRot;

    public EnvironmentData(List<string> constructedName, List<float> constructedpos, List<float> constructedRot)
    {
        this.constructedName = constructedName;
        this.constructedpos = constructedpos;
        this.constructedRot = constructedRot;
    }
        
}
