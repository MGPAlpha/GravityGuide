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


    void Start()
    {
        if (runAtStart) StartTimer();
    }

    public void StartTimer() {
        timerOn = true;
        timeElapsed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerOn) {
            timeElapsed += Time.deltaTime;
            if (timeElapsed > timerLength) {
                timerOn = false;
                onTimeout.Invoke();
            }
        }
    }

    private void OnDrawGizmos()
    {
        CustomGizmos.DrawEventTargets(transform.position, onTimeout, Color.yellow);
        Gizmos.DrawIcon(transform.position, "Timer");
    }
}
