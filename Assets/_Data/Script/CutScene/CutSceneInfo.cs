using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
[System.Serializable]
public class CutSceneInfo
{
    public string name;
    public Playable Playable;
    public PlayableDirector director;
}
