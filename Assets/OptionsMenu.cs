using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public static float masterVolume {
        get;
        private set;
    } = 1;

    public static bool subtitles {
        get;
        private set;
    } = true;

    public static bool speedrunClock {
        get;
        private set;
    } = false;

    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Toggle subtitlesToggle;
    [SerializeField] private Toggle speedrunToggle;

    private static bool optionsReady = false;
    
    // Start is called before the first frame update
    void Start()
    {
        if (!optionsReady) {
            if (PlayerPrefs.HasKey("_MasterVolume")) {
                masterVolume = PlayerPrefs.GetFloat("_MasterVolume");
            } else {
                PlayerPrefs.SetFloat("_MasterVolume", 1);
                masterVolume = 1;
            }
            AudioListener.volume = masterVolume;
            
            if (PlayerPrefs.HasKey("_Subtitles")) {
                subtitles = PlayerPrefs.GetFloat("_Subtitles") == 1;
            } else {
                PlayerPrefs.SetFloat("_Subtitles", 1);
                subtitles = true;
            }

            if (PlayerPrefs.HasKey("_Speedrun")) {
                speedrunClock = PlayerPrefs.GetFloat("_Speedrun") == 1;
            } else {
                PlayerPrefs.SetFloat("_Speedrun", 0);
                speedrunClock = false;
            }

            optionsReady = true;
        }
        
        volumeSlider.value = masterVolume;
        subtitlesToggle.isOn = subtitles;
        speedrunToggle.isOn = speedrunClock;

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateVolume(float vol) {
        masterVolume = vol;
        PlayerPrefs.SetFloat("_MasterVolume", masterVolume);
        AudioListener.volume = masterVolume;
    }

    public void UpdateSubtitles(bool sub) {
        subtitles = sub;
        PlayerPrefs.SetFloat("_Subtitles", sub ? 1 : 0);
        if (!subtitles && SubtitleManager._sm) {
            SubtitleManager._sm.gameObject.SetActive(false);
        }
    }

    public void UpdateSpeedrun(bool sp) {
        speedrunClock = sp;
        PlayerPrefs.SetFloat("_Speedrun", sp ? 1 : 0);
    }
}
