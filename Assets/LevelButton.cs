using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private int buildIndex = 1;
    
    [SerializeField] private TextMeshProUGUI levelNum; 
    [SerializeField] private TextMeshProUGUI levelName; 

    private Button _btn;

    // Start is called before the first frame update
    void Start()
    {
        _btn = GetComponent<Button>();
        
        if (buildIndex > PlayerPrefs.GetInt("maxLevelProgress")) {
            levelNum.text = "?";
            levelName.text = "?????";
            _btn.interactable = false;
        }
    }

    public void LoadLevel() {
        PlayerPrefs.SetInt("levelProgress", buildIndex);
        MenuManager._mm.Continue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
