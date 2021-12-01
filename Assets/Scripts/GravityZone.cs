using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityZone : MonoBehaviour
{
    [SerializeField] private bool startOn = true;
    
    [SerializeField] private LayerMask affectedLayers;
    [SerializeField] private Vector2 gravityDir = Vector2.down;

    [SerializeField] private float gravityFadeTime = .5f;

    private AudioSource _as;

    [SerializeField] private AudioClip powerOnEffect;
    [SerializeField] private AudioClip powerDownEffect;

    private bool gravityOn;
    private float gravityShaderProgress = 1;
    
    private SpriteRenderer sp;

    // Start is called before the first frame update
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        gravityOn = startOn;
        sp.material.SetFloat("_GravityOn", gravityOn ? 1 : 0);
        gravityShaderProgress = gravityOn ? 1 : 0;
        TryGetComponent<AudioSource>(out _as);
    }

    // Update is called once per frame
    void Update()
    {
        gravityShaderProgress = Mathf.MoveTowards(gravityShaderProgress, gravityOn ? 1 : 0, Time.deltaTime / gravityFadeTime);
        float arrowDir = Vector2.Angle(gravityDir, Vector2.down);
        if (gravityDir.x > 0) arrowDir = 360 - arrowDir;
        sp.material.SetFloat("_ArrowAngle", arrowDir * 3.14f / 180);
        sp.material.SetFloat("_GravityOn", gravityShaderProgress);
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (gravityOn && (affectedLayers == (affectedLayers | (1 << other.gameObject.layer)))) {
            GravityObject g = other.GetComponent<GravityObject>();
            float oldGravity = g.personalGravity.magnitude;
            Vector2 newGravity = oldGravity * gravityDir.normalized;
            g.personalGravity = newGravity;
        }
    }

    public void SetOn(bool on) {
        bool changed = on != gravityOn;
        gravityOn = on;
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

    public void ToggleOn() {
        SetOn(!gravityOn);
    }
}
