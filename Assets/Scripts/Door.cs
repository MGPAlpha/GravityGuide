using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    
    private Animator an;

    private AudioSource aud;
    
    [SerializeField] private bool open = false;
    [SerializeField] private bool locked = true;

    [SerializeField] private AudioClip openEffect;
    [SerializeField] private AudioClip closeEffect;


    private bool proximity = false;

    public void Open() {
        if (open) return;
        open = true;
        an.SetBool("open", true);
        if (locked && aud) {
            aud.clip = openEffect;
            aud.Play();
        }
    }

    public void Close() {
        if (!open) return;
        open = false;
        an.SetBool("open", false);
        if (locked && aud) {
            aud.clip = closeEffect;
            aud.Play();
        }
    }

    public void Toggle() {
        open = !open;
        an.SetBool("open", open);
        if (locked && aud) {
            aud.clip = open ? openEffect : closeEffect;
            aud.Play();
        }
    }

    public void Lock() {
        if (locked) return;
        locked = true;
        an.SetBool("locked", true);
        an.SetBool("open", open);
        if (!open && proximity && aud) {
            aud.clip = closeEffect;
            aud.Play();
        }
    }

    public void Unlock() {
        if (!locked) return;
        locked = false;
        an.SetBool("locked", false);
        an.SetBool("open", proximity);
        if (!open && proximity && aud) {
            aud.clip = openEffect;
            aud.Play();
        }
    }

    public void TriggerProximity() {
        proximity = true;
        if (!locked) an.SetBool("open", true);
        if (!locked && aud) {
            aud.clip = openEffect;
            aud.Play();
        }
    }

    public void UntriggerProximity() {
        proximity = false;
        if (!locked) an.SetBool("open", false);
        if (!locked && aud) {
            aud.clip = closeEffect;
            aud.Play();
        }
    }

    void Start()
    {
        an = GetComponent<Animator>();
        an.SetBool("locked", locked);
        an.SetBool("open", open);
        an.Play("Door " + (locked ? (open ? "Open" : "Locked") : "Closed"));
        TryGetComponent<AudioSource>(out aud);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
