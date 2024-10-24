using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventTimer : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField] private float timerLength = 5f; 
    private float timeElapsed = 0;
    private bool timerOn = false;

    [SerializeField] private bool runAtStart = false;

    [SerializeField] private UnityEvent onTimeout;

    [SerializeField] private bool doSound = false;
    [SerializeField] private AudioClip timerTick;
    [SerializeField] private AudioClip timerDing;

    private AudioSource _as;
    private SpriteRenderer _re;


    void Start()
    {
        if (runAtStart) StartTimer();
        TryGetComponent<AudioSource>(out _as);
        TryGetComponent<SpriteRenderer>(out _re);
    }

    public void StartTimer() {
        bool alreadyStarted = timerOn;
        timerOn = true;
        timeElapsed = 0;
        if (_as && doSound && !alreadyStarted) {
            _as.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timerOn) {
            timeElapsed += Time.deltaTime;
            if (timeElapsed > timerLength) {
                timerOn = false;
                onTimeout.Invoke();
                if (_as) {
                    _as.Stop();
                    if (timerDing && doSound) _as.PlayOneShot(timerDing);
                }
            }
        }
        // Debug.Log(_re);
        // Debug.Log(_re.enabled);
        if (_re && _re.enabled) {
            // Debug.Log("changing material");
            if (timerOn) {
                _re.material.SetFloat("_Fill", timeElapsed / timerLength);
                Debug.Log("setting fill to " + (timeElapsed / timerLength));
            } else {
                _re.material.SetFloat("_Fill", 0f);
            }
        }
    }

    private void OnDrawGizmos()
    {
        CustomGizmos.DrawEventTargets(transform.position, onTimeout, Color.yellow);
        if (!TryGetComponent<SpriteRenderer>(out SpriteRenderer re) || !re.enabled) Gizmos.DrawIcon(transform.position, "Timer");
    }
}
