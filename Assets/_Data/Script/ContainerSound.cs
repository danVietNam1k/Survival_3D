using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ContainerSound", menuName = "ScriptableObjects/ContainerSound", order = 1 )]
public class ContainerSound : ScriptableObject
{
    [Header("Music Background")]
    public List<AudioClip> musicsBackground;
    [Header("Axe")]
    public AudioClip[] axeChoppingTree;
    public AudioClip[] axeAtack;
    public AudioClip axeSwing;


    [Header("Player Action")]
    public AudioClip playerWalk;
    public AudioClip playerJump;
    public AudioClip playerSprint;

}


