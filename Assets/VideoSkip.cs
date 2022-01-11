using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VideoSkip : MonoBehaviour
{
    private UnityEngine.Video.VideoPlayer _vp;

    [SerializeField] private UnityEvent onSkip;
    
    // Start is called before the first frame update
    void Start()
    {
        _vp = GetComponent<UnityEngine.Video.VideoPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_vp.isPlaying && Input.GetKeyDown(KeyCode.Backspace)) {
            _vp.Stop();
            GetComponent<Renderer>().enabled = false;
            if (onSkip.GetPersistentEventCount() > 0) onSkip.Invoke();
        }
    }
}
