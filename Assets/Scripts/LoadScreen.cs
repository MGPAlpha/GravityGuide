using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadScreen : MonoBehaviour
{
    [SerializeField] private bool startFilled = true;
    [SerializeField] private float fillSpeed = 1.5f;
    [SerializeField] private bool openOnLoad = true;

    private AdvanceLevel triggerAdvance = null;

    private float fill;
    private float fillTarget;

    private Image _im;

    // Start is called before the first frame update
    void Start()
    {
        _im = GetComponent<Image>();
        fillTarget = startFilled ? 1 : 0;
        fill = fillTarget;
        _im.material.SetFloat("_Fill", fillTarget);
        if (openOnLoad) fillTarget = 0;
    }

    // Update is called once per frame
    void Update()
    {
        fill = Mathf.MoveTowards(fill, fillTarget, fillSpeed * Time.deltaTime);
        _im.materialForRendering.SetFloat("_Fill", fill);
        if (triggerAdvance && fill == fillTarget) {
            triggerAdvance.LoadNextLevelAsync();
            triggerAdvance = null;
        }
    }

    public void FillAndTriggerLevel(AdvanceLevel al) {
        fillTarget = 1;
        triggerAdvance = al;
    }
}
