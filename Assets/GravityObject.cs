using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityObject : MonoBehaviour
{
    public Vector2 personalGravity;

    private Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        if (personalGravity == Vector2.zero) personalGravity = Physics.gravity;
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
