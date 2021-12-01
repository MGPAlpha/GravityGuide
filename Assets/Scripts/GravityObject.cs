using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityObject : MonoBehaviour
{
    [SerializeField] public bool autoStartGravity = true;

    public Vector2 personalGravity;

    protected Rigidbody2D rb;

    private AudioSource aud;

    [SerializeField] private AudioClip[] hitEffects;
    
    // Start is called before the first frame update
    protected void Start()
    {
        if (autoStartGravity) personalGravity = Physics.gravity;
        rb = GetComponent<Rigidbody2D>();
        TryGetComponent<AudioSource>(out aud);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        rb.velocity += personalGravity * Time.fixedDeltaTime;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (aud && hitEffects.Length > 0) {
            aud.PlayOneShot(hitEffects[Random.Range(0,hitEffects.Length)]);
        }
    }
}
