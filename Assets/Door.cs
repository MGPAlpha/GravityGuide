using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    
    private Animator an;
    
    [SerializeField] private bool open = false;

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

    void Start()
    {
        an = GetComponent<Animator>();
        an.SetBool("open", open);
        an.Play("Door " + (open ? "Open" : "Closed"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
