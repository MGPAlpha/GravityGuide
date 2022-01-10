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

    [SerializeField] private Slider volumeSlider;

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
            optionsReady = true;
        }
        
        volumeSlider.value = masterVolume;

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
}
