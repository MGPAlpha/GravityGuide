using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    
    private Animator an;
    
    [SerializeField] private bool open = false;
    [SerializeField] private bool locked = true;

    private bool proximity = false;

    public void Open() {
        open = true;
        an.SetBool("open", true);
    }

    public void Close() {
        open = false;
        an.SetBool("open", false);
    }

    public void Toggle() {
        open = !open;
        an.SetBool("open", open);
    }

    public void Lock() {
        locked = true;
        an.SetBool("locked", true);
        an.SetBool("open", open);
    }

    public void Unlock() {
        locked = false;
        an.SetBool("locked", false);
        an.SetBool("open", proximity);
    }

    public void TriggerProximity() {
        proximity = true;
        if (!locked) an.SetBool("open", true);
    }

    public void UntriggerProximity() {
        proximity = false;
        if (!locked) an.SetBool("open", false);
    }

    void Start()
    {
        an = GetComponent<Animator>();
        an.SetBool("locked", locked);
        an.SetBool("open", open);
        an.Play("Door " + (locked ? (open ? "Open" : "Locked") : "Closed"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
