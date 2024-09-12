using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VideoScreen : MonoBehaviour
{
    private UnityEngine.Video.VideoPlayer _vp;
    private Animator _anim;

    private bool isPlaying = false;

    [SerializeField] private UnityEvent onFinishOrSkip;
    
    // Start is called before the first frame update
    void Start()
    {
        _vp = GetComponentInChildren<UnityEngine.Video.VideoPlayer>();
        _vp.loopPointReached += Finish;
        _anim = GetComponentInChildren<Animator>();
    }

    public void Play() {
        isPlaying = true;
        _anim.SetTrigger("TurnOn");
    }

    public void NotifyPowerAnimationFinished() {
        _vp.Play();
    }

    public void Finish(UnityEngine.Video.VideoPlayer vp) {
        isPlaying = false;
        _vp.Stop();
        _anim.SetTrigger("TurnOff");
        // GetComponent<Renderer>().enabled = false;
        if (onFinishOrSkip.GetPersistentEventCount() > 0) onFinishOrSkip.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying && Input.GetKeyDown(KeyCode.Backspace)) {
            Finish(_vp);
        }
    }
}
