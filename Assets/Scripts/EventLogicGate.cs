using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventLogicGate : MonoBehaviour
{
    public bool[] inputs;

    private bool activated;

    [SerializeField] private bool isAndGate = false;

    [SerializeField] private UnityEvent onActivate;
    [SerializeField] private UnityEvent onDeactivate;
    
    // Start is called before the first frame update
    void Start()
    {
        activated = isAndGate;
        for (int i = 0; i < inputs.Length; i++) {
            if (inputs[i] == !isAndGate) {
                activated = !isAndGate;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CheckInputs() {
        bool oldActivated = activated;
        activated = isAndGate;
        for (int i = 0; i < inputs.Length; i++) {
            if (inputs[i] == !isAndGate) {
                activated = !isAndGate;
                break;
            }
        }
        if (oldActivated != activated) {
            UnityEvent relevantEvent = activated ? onActivate : onDeactivate;
            if (relevantEvent.GetPersistentEventCount() > 0) relevantEvent.Invoke();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, isAndGate ? "And Gate" : "Or Gate");
        CustomGizmos.DrawEventTargets(transform.position, onDeactivate, Color.red);
        CustomGizmos.DrawEventTargets(transform.position, onActivate, Color.green);
    }

    public void SetInput(int index, bool value) {
        inputs[index] = value;
        CheckInputs();
    }

    public void ActivateInput(int index) {
        SetInput(index, true);
    }

    public void DeactivateInput(int index) {
        SetInput(index, false);
    }

    public void ToggleInput(int index) {
        SetInput(index, !inputs[index]);
    }
}
