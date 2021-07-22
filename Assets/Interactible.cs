using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactible : MonoBehaviour
{
    [SerializeField] private bool singleUse = true;
    [SerializeField] private bool active = true;
    [SerializeField] private int priority = 0;

    [SerializeField] private UnityEvent onInteract;
    [SerializeField] private UnityEvent onTarget;
    [SerializeField] private UnityEvent onDetarget;
    
    public bool CanTarget() {
        return active;
    }

    public bool Interact() { // Returns whether to keep target;
        if (singleUse) {
            active = false;
        }
        onInteract.Invoke();
        return active;
    }

    public void Target() {
        if (CanTarget()) {
            onTarget.Invoke();
        }
    }

    public void Detarget() {
        onDetarget.Invoke();
    }

    public int GetPriority() {
        return priority;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
