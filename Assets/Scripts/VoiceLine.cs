using UnityEngine;

[CreateAssetMenu(fileName = "Voice Line", menuName = "Audio/Voice Line", order = 1)]
public class VoiceLine : ScriptableObject
{
    public AudioClip clip;

    public float startTime;
    public float endTime;

    [Multiline(15)]
    public string subtitles;
}
