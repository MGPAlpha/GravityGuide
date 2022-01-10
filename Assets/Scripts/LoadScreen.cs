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
        "Gravity Fields don't cancel you momentum, so if you build up enough speed you can pass through them, against the flow."
    }; 

    private static string currentTip = null;

    private AdvanceLevel triggerAdvance = null;

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
        if (triggerAdvance && fill == fillTarget) {
            triggerAdvance.LoadNextLevelAsync();
            triggerAdvance = null;
        }
    }

    public void FillAndTriggerLevel(AdvanceLevel al) {
        fillTarget = 1;
        triggerAdvance = al;
        NewTip();
    }
}
