using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SubtitleManager : MonoBehaviour
{
    public static SubtitleManager _sm {
        get;
        private set;
    }
    
    private void Awake()
    {
        if (_sm == null || _sm.gameObject == null) {
            _sm = this;
            gameObject.SetActive(false);
        } else {
            Destroy(this);
        }
    }

    [SerializeField] private TextMeshProUGUI subtitleText;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    public void ShowSubtitle(string sub) {
        if (!OptionsMenu.subtitles) return;
        gameObject.SetActive(true);
        subtitleText.text = sub;
    }
}
