using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoomEffect : MonoBehaviour
{
    private Image _re;

    [SerializeField] private float transitionSpeed = .6f;
    [SerializeField] private float transitionTarget = 0;
    private float transitionProgress;

    // Start is called before the first frame update
    void Start()
    {
        _re = GetComponent<Image>();
        transitionProgress = transitionTarget;
    }

    // Update is called once per frame
    void Update()
    {
        transitionProgress = Mathf.MoveTowards(transitionProgress, transitionTarget, transitionSpeed * Time.deltaTime);
        _re.material.SetFloat("_ZoomProgress", transitionProgress);
    }

    public void SetTransitionTarget(float target) {
        transitionTarget = target;
    }

    public bool TargetReached() {
        return transitionProgress == transitionTarget;
    }
}
