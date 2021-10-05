using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinorGravityFlux : MonoBehaviour
{
    private GravityObject g;
    void Start()
    {
        g = GetComponent<GravityObject>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        g.personalGravity = Quaternion.Euler(0,0,Random.Range(-1f, 1f) * 5 * Time.fixedDeltaTime) * g.personalGravity;
    }
}
