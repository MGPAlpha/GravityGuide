using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadScreen : MonoBehaviour
{
    [SerializeField] private bool startFilled = true;
    [SerializeField] private float fillSpeed = 1.5f;
    [SerializeField] private bool openOnLoad = true;

    [SerializeField] private TextMeshProUGUI tip;

    private static string[] tips = {
        "Hold shift while aiming your Gravity Manipulators to aim at precise angles.",
        "If you ever get stuck during a level, you can restart it from the pause menu.",
        "Crate Dispensers can only dispense one crate at a time. If you request a new crate, the old one will automatically disintegrate.",
        "Gravity Fields don't cancel your momentum, so if you build up enough speed you can pass through them, against the flow.",
        "While inside a Gravity Field, you cannot alter your own gravity."
    }; 

    private static string currentTip = null;

    private AdvanceLevel triggerAdvance = null;
    private bool triggerMenu = false;
    private bool triggerQuit = false;

    private float fill;
    private float fillTarget;

    private Image _im;

    // Start is called before the first frame update
    void Start()
    {
        _im = GetComponent<Image>();
        if (tip) {
            if (currentTip == null) NewTip();
            tip.text = currentTip;
        }
        fillTarget = startFilled ? 1 : 0;
        fill = fillTarget;
        _im.material.SetFloat("_Fill", fillTarget);
        if (openOnLoad) fillTarget = 0;
    }

    private void NewTip() {
        currentTip = tips[Random.Range(0,tips.Length)];
        tip.text = currentTip;
    }

    // Update is called once per frame
    void Update()
    {
        fill = Mathf.MoveTowards(fill, fillTarget, fillSpeed * Time.unscaledDeltaTime);
        _im.materialForRendering.SetFloat("_Fill", fill);
        _im.raycastTarget = fill != 0;
        AudioListener.volume = Mathf.Lerp(0, OptionsMenu.masterVolume, 1 - fill);
        if (triggerQuit  && fill == fillTarget) {
            Application.Quit();
        }
        if (triggerAdvance && fill == fillTarget) {
            triggerAdvance.LoadNextLevelAsync();
            triggerAdvance = null;
        }
        if (triggerMenu && fill == fillTarget) {
            MenuManager._mm.ReadyToLoad();
            triggerMenu = false;
        }
    }

    public void FillAndTriggerLevel(AdvanceLevel al) {
        fillTarget = 1;
        triggerAdvance = al;
        NewTip();
    }

    public void FillAndTriggerMenu() {
        fillTarget = 1;
        triggerMenu = true;
        NewTip();
    }

    public void FillAndQuit() {
        fillTarget = 1;
        triggerQuit = true;
        NewTip();
    }
}
