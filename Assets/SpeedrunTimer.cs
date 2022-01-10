using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpeedrunTimer : MonoBehaviour
{
    public static SpeedrunTimer _st {
        get;
        private set;
    }
    
    private void Awake()
    {
        if (_st == null || _st.gameObject == null) {
            _st = this;
        } else {
            Destroy(this);
        }
    }
    
    Image _im;
    TextMeshProUGUI _tmp;

    private static float _time = 0;

    [SerializeField] private bool timerActive = true;
    
    private static bool prefsChecked = false;

    // Start is called before the first frame update
    void Start()
    {
        _im = GetComponent<Image>();
        _tmp = GetComponentInChildren<TextMeshProUGUI>();

        if (!prefsChecked) {
            if (PlayerPrefs.HasKey("_SpeedrunTime")) {
                _time = PlayerPrefs.GetFloat("_SpeedrunTime");
            } else {
                PlayerPrefs.SetFloat("_SpeedrunTime", 0);
                _time = 0;
            }
            prefsChecked = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        _im.enabled = OptionsMenu.speedrunClock;
        _tmp.enabled = OptionsMenu.speedrunClock;

        if (timerActive) {
            _time += Time.unscaledDeltaTime;
        }

        System.TimeSpan t = System.TimeSpan.FromSeconds(_time);
        string timeString = string.Format("{0:D1}:{1:D2}:{2:D2}.{3:D3}", t.Hours, t.Minutes, t.Seconds, t.Milliseconds);
        _tmp.text = timeString;
    }

    public void ActivateTimer(bool act) {
        timerActive = act;
        PlayerPrefs.SetFloat("_SpeedrunTime", _time);
    }

    public static void ResetSpeedrunTimer() {
        _time = 0;
        PlayerPrefs.SetFloat("_SpeedrunTime", _time);
    }

}

