using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBarrier : MonoBehaviour
{
    [SerializeField] private bool startOn = true;

    [SerializeField] private float fadeTime = .5f;

    private bool barrierOn;
    private float shaderProgress = 1;
    
    private SpriteRenderer sp;
    private Collider2D col;

    private AudioSource _as;

    [SerializeField] private AudioClip powerOnEffect;
    [SerializeField] private AudioClip powerDownEffect;

    // Start is called before the first frame update
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        barrierOn = startOn;
        sp.material.SetFloat("_Active", barrierOn ? 1 : 0);
        col.enabled = barrierOn;
        shaderProgress = barrierOn ? 1 : 0;
        TryGetComponent<AudioSource>(out _as);
    }

    // Update is called once per frame
    void Update()
    {
        shaderProgress = Mathf.MoveTowards(shaderProgress, barrierOn ? 1 : 0, Time.deltaTime / fadeTime);
        sp.material.SetFloat("_Active", shaderProgress);
    }

    public void SetOn(bool on) {
        bool changed = on != barrierOn;
        barrierOn = on;
        col.enabled = on;
        if (_as && changed) {
            AudioClip clip = on ? powerOnEffect : powerDownEffect;
            _as.Stop();
            _as.clip = clip;
            if (clip)_as.Play();
        }
    }

    public void TurnOn() {
        SetOn(true);
    }

    public void TurnOff() {
        SetOn(false);
    }

    public void Toggle() {
        SetOn(!barrierOn);
    }
}
