using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityObject : MonoBehaviour
{
    [SerializeField] public bool autoStartGravity = true;

    public Vector2 personalGravity;

    protected Rigidbody2D rb;
    
    // Start is called before the first frame update
    protected void Start()
    {
        if (autoStartGravity) personalGravity = Physics.gravity;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        rb.velocity += personalGravity * Time.fixedDeltaTime;
    }
}
