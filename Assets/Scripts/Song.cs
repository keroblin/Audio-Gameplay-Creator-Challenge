using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Conductor;
[CreateAssetMenu(fileName = "Song", menuName = "ScriptableObjects/Song")]
public class Song : ScriptableObject
{
    public AudioClip song;
    public float bpm;
    public float startOffset;

    public List<BeatType> events;
}
