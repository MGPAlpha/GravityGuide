using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;

public class VoiceLinePlayer : MonoBehaviour
{
    private static VoiceLinePlayer activePlayer;
    
    [SerializeField] private VoiceLine[] clips;

    [SerializeField] private UnityEvent onFinish;

    private AudioSource audioSource;

    private bool playing;
    private int playIndex = -1;
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playing) {
            if (playIndex < 0 || audioSource.time > clips[playIndex].endTime) {
                playIndex++;
                audioSource.Stop();
                if (playIndex < clips.Length) {
                    audioSource.clip = clips[playIndex].clip;
                    audioSource.time = clips[playIndex].startTime;
                    audioSource.Play();
                    GenerateSubtitles(clips[playIndex].subtitles);
                    SubtitleManager._sm.ShowSubtitle(subs[0].text);
                } else {
                    playing = false;
                    SubtitleManager._sm.Hide();
                    if (onFinish.GetPersistentEventCount() > 0) onFinish.Invoke();
                }
            }
            if (subtitleIndex < subs.Length - 1 && audioSource.time >= subs[subtitleIndex + 1].timestamp) {
                subtitleIndex++;
                SubtitleManager._sm.ShowSubtitle(subs[subtitleIndex].text);
            }
            if (Input.GetKeyDown(KeyCode.Backspace)) {
                EndEarly();
            }
        }
    }

    public void Play() {
        if (activePlayer) {
            activePlayer.EndEarly();
        }
        playIndex = -1;
        playing = true;
        activePlayer = this;
    }

    public void EndEarly() {
        playing = false;
        SubtitleManager._sm.Hide();
        audioSource.Stop();
        if (onFinish.GetPersistentEventCount() > 0) onFinish.Invoke();
    }

    private class Subtitle {
        public string text;
        public float timestamp;
    }

    private Subtitle[] subs;

    private int subtitleIndex = 0;

    private void GenerateSubtitles(string subsText) {
        string regexp = @"<t=(\d+)>";
        string regexp2 = @"<t=\d+>";
        string[] subsArray = Regex.Split(subsText, regexp2);
        MatchCollection stamps = Regex.Matches(subsText, regexp, RegexOptions.None);
        subs = new Subtitle[subsArray.Length];
        for (int i = 0; i < subsArray.Length; i++) {
            subs[i] = new Subtitle();
            subs[i].text = subsArray[i];
            if (i == 0) {
                subs[i].timestamp = 0;
                continue;
            }
            subs[i].timestamp = float.Parse(stamps[i-1].Groups[1].Value);
        }
    }

    private void OnDrawGizmos()
    {
        CustomGizmos.DrawEventTargets(this.transform.position, onFinish, Color.magenta);
    }
}
