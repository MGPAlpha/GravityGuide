using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class AdvanceLevel : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadNextLevel() {
        if (SpeedrunTimer._st) {
            SpeedrunTimer._st.ActivateTimer(false);
        }
        if (MenuManager._mm.loadScreen) {
            MenuManager._mm.loadScreen.FillAndTriggerLevel(this);
        } else SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadNextLevelAsync() {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }


}
