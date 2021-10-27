using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disintegrator : MonoBehaviour
{
    [SerializeField] private bool on = true;
    [SerializeField] private float activateTime = .5f;
    private float shaderProgress = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        if (on) shaderProgress = activateTime;
    }

    // Update is called once per frame
    void Update()
    {
        shaderProgress = Mathf.MoveTowards(shaderProgress, on ? activateTime : 0, Time.deltaTime);
        GetComponent<Renderer>().material.SetFloat("_Active", Mathf.InverseLerp(0, activateTime, shaderProgress));
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Got it");
        other.GetComponent<DissolveDestroy>().Dissolve();
    }

    public void SetOn(bool isOn) {
        on = isOn;
        GetComponent<Collider2D>().enabled = on;
    }

    public void TurnOn() {
        SetOn(true);
    }

    public void TurnOff() {
        SetOn(false);
    }

    public void Toggle() {
        SetOn(!on);
    }
}
