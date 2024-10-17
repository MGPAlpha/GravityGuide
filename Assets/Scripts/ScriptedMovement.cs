using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScriptedMovement : MonoBehaviour
{

    [SerializeField] private Vector2 movement;
    [SerializeField] private float time = 1;
    [SerializeField] private bool startOnAwake = false;
    [SerializeField] private bool repeat;
    [SerializeField] private bool pingPong;
    [SerializeField] private bool delay;
    [SerializeField] private float delayTime;

    [SerializeField] private UnityEvent onFinishMovement; 

    private enum MovementState {
        Idle,
        Moving,
        Reversing,
        Delay,
        ReverseDelay,
    }

    private MovementState state = MovementState.Idle;
    private float stateTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (startOnAwake) {
            if (delay) {
                state = MovementState.Delay;
            } else {
                state = MovementState.Moving;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (state == MovementState.Delay || state == MovementState.ReverseDelay) {
            stateTime += Time.deltaTime;
            if (stateTime > delayTime) {
                stateTime = 0;
                if (state == MovementState.Delay) {
                    state = MovementState.Moving;
                } else {
                    state = MovementState.Reversing;
                }
            }
        } else if (state == MovementState.Moving || state == MovementState.Reversing) {
            Vector2 progress = movement * stateTime / time;
            stateTime += Time.deltaTime;
            stateTime = Mathf.Min(stateTime, time);
            Vector2 next = movement * stateTime / time;
            Vector3 dx = next - progress;
            if (state == MovementState.Reversing) {
                dx = -dx;
            }
            transform.position += dx;
            if (stateTime >= time) {
                if (!repeat && (!pingPong || state == MovementState.Reversing)) {
                    state = MovementState.Idle;
                    onFinishMovement.Invoke();
                } else if (delay && (!pingPong || state == MovementState.Reversing)) {
                    state = MovementState.Delay;
                    onFinishMovement.Invoke();
                } else if (delay) {
                    state = MovementState.ReverseDelay;
                } else if (!pingPong || state == MovementState.Reversing) {
                    state = MovementState.Moving;
                } else {
                    state = MovementState.Reversing;
                }
                stateTime = 0;
            }
        }
    }

    public void Move() {
        if (delay) {
            state = MovementState.Delay;
        } else {
            state = MovementState.Moving;
        }
        stateTime = 0;
    }

    public void Stop() 
    {
        state = MovementState.Idle;
        stateTime = 0;
    }
}
