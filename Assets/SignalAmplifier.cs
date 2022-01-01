using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalAmplifier : MonoBehaviour
{

    private Renderer _re;

    private Vector4 rotSpeeds;

    private Vector4 activations = new Vector4(0,0,0,1);

    // Start is called before the first frame update
    void Start()
    {
        _re = GetComponent<Renderer>();
        rotSpeeds = _re.material.GetVector("_RotSpeeds");
        _re.material.SetVector("_RotSpeeds", new Vector4(0,0,0,rotSpeeds.w));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateRing1() {
        if (activations.z != 0) return;

        Vector4 startTimes = _re.material.GetVector("_RotStarts");
        Vector4 matRotSpeeds = _re.material.GetVector("_RotSpeeds");

        float currTime = startTimes.z - rotSpeeds.z * Time.time;

        startTimes = new Vector4(startTimes.x, startTimes.y, currTime, startTimes.w);
        _re.material.SetVector("_RotStarts", startTimes);
        matRotSpeeds = new Vector4(matRotSpeeds.x, matRotSpeeds.y, rotSpeeds.z, matRotSpeeds.w);
        _re.material.SetVector("_RotSpeeds", matRotSpeeds);

        activations.z = 1;
    }

    public void DeactivateRing1() {
        if (activations.z == 0) return;

        Vector4 startTimes = _re.material.GetVector("_RotStarts");
        Vector4 matRotSpeeds = _re.material.GetVector("_RotSpeeds");

        float currTime = Time.time * rotSpeeds.z + startTimes.z;

        startTimes = new Vector4(startTimes.x, startTimes.y, currTime, startTimes.w);
        _re.material.SetVector("_RotStarts", startTimes);
        matRotSpeeds = new Vector4(matRotSpeeds.x, matRotSpeeds.y, 0, matRotSpeeds.w);
        _re.material.SetVector("_RotSpeeds", matRotSpeeds);

        activations.z = 0;
    }

    public void ActivateRing2() {
        if (activations.y != 0) return;

        Vector4 startTimes = _re.material.GetVector("_RotStarts");
        Vector4 matRotSpeeds = _re.material.GetVector("_RotSpeeds");

        float currTime = startTimes.y - rotSpeeds.y * Time.time;

        startTimes = new Vector4(startTimes.x, currTime, startTimes.z, startTimes.w);
        _re.material.SetVector("_RotStarts", startTimes);
        matRotSpeeds = new Vector4(matRotSpeeds.x, rotSpeeds.y, matRotSpeeds.z, matRotSpeeds.w);
        _re.material.SetVector("_RotSpeeds", matRotSpeeds);

        activations.y = 1;
    }

    public void DeactivateRing2() {
        if (activations.y == 0) return;

        Vector4 startTimes = _re.material.GetVector("_RotStarts");
        Vector4 matRotSpeeds = _re.material.GetVector("_RotSpeeds");

        float currTime = Time.time * rotSpeeds.y + startTimes.y;

        startTimes = new Vector4(startTimes.x, currTime, startTimes.z, startTimes.w);
        _re.material.SetVector("_RotStarts", startTimes);
        matRotSpeeds = new Vector4(matRotSpeeds.x, 0, matRotSpeeds.z, matRotSpeeds.w);
        _re.material.SetVector("_RotSpeeds", matRotSpeeds);

        activations.y = 0;
    }

    public void ActivateRing3() {
        if (activations.x != 0) return;

        Vector4 startTimes = _re.material.GetVector("_RotStarts");
        Vector4 matRotSpeeds = _re.material.GetVector("_RotSpeeds");

        float currTime = startTimes.x - rotSpeeds.x * Time.time;

        startTimes = new Vector4(currTime, startTimes.y, startTimes.z, startTimes.w);
        _re.material.SetVector("_RotStarts", startTimes);
        matRotSpeeds = new Vector4(rotSpeeds.x, matRotSpeeds.y, matRotSpeeds.z, matRotSpeeds.w);
        _re.material.SetVector("_RotSpeeds", matRotSpeeds);

        activations.x = 1;
    }

    public void DeactivateRing3() {
        if (activations.x == 0) return;

        Vector4 startTimes = _re.material.GetVector("_RotStarts");
        Vector4 matRotSpeeds = _re.material.GetVector("_RotSpeeds");

        float currTime = Time.time * rotSpeeds.x + startTimes.x;

        startTimes = new Vector4(currTime, startTimes.y, startTimes.z, startTimes.w);
        _re.material.SetVector("_RotStarts", startTimes);
        matRotSpeeds = new Vector4(0, matRotSpeeds.y, matRotSpeeds.z, matRotSpeeds.w);
        _re.material.SetVector("_RotSpeeds", matRotSpeeds);

        activations.x = 0;
    }
}
