using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
[CreateAssetMenu(fileName = "CutSceneData", menuName = "ScriptableObjects/CutSceneData", order = 1)]
public class CutSceneData : ScriptableObject
{
 public List<CutSceneInfo> listCutScene = new List<CutSceneInfo>();

}

