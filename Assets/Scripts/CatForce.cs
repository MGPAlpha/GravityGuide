using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatForce : MonoBehaviour
{
    private Rigidbody2D _rb;
    private GravityObject _g;

    [SerializeField] private float minForceInterval = 1;
    [SerializeField] private float maxForceInterval = 3;
    [SerializeField] private float minForce = 1;
    [SerializeField] private float maxForce = 2;

    private float timer = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _g = GetComponent<GravityObject>();
        timer = Random.Range(minForceInterval, maxForceInterval);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer -= Time.fixedDeltaTime;

        if (timer <= 0) {
            float forceAngle = Random.Range(-90,90);
            Vector2 forceDir = Quaternion.Euler(0,0,forceAngle) * -_g.personalGravity;
            _rb.AddForce(forceDir.normalized * Random.Range(minForce, maxForce));
            timer += Random.Range(minForceInterval, maxForceInterval);
        }
    }
}
