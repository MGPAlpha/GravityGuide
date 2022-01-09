using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScreenFadeTrigger : MonoBehaviour
{
    [SerializeField] private ZoomEffect effect;
    [SerializeField] private UnityEvent onFadeEnd;
    
    [SerializeField] private bool startActive;

    private bool eventActive;

    // Start is called before the first frame update
    void Start()
    {
        eventActive = startActive;
    }

    // Update is called once per frame
    void Update()
    {
        if (eventActive && effect) {
            if (effect.TargetReached()) {
                if (onFadeEnd.GetPersistentEventCount() > 0) onFadeEnd.Invoke();
                eventActive = false;
            }
        }
    }

    public void SetEventActive(bool active) {
        eventActive = active;
    }
}
