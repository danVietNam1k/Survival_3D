using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rename : MonoBehaviour
{
    [SerializeField] string _name;
    // Start is called before the first frame update
 void OnValidate()
    {
        for(int i = 0; i< transform.childCount; i++) 
            {
            transform.GetChild(i).name = _name + (i+1);
            }
    }
}
