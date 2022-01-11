using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockCheat : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private int codeProgress = 0;

    private KeyCode[] code = {
        KeyCode.UpArrow,
        KeyCode.UpArrow,
        KeyCode.DownArrow,
        KeyCode.DownArrow,
        KeyCode.LeftArrow,
        KeyCode.RightArrow,
        KeyCode.LeftArrow,
        KeyCode.RightArrow,
        KeyCode.B,
        KeyCode.A,
        KeyCode.Return
    };

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown) {
            if (Input.GetKeyDown(code[codeProgress])) {
                codeProgress++;
                if (codeProgress == code.Length) {
                    PlayerPrefs.SetInt("maxLevelProgress", 12);
                    MenuManager._mm.Menu();
                }
            } else {
                codeProgress = 0;
            }
        }
    }
}
