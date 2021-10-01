using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityZone : MonoBehaviour
{
    [SerializeField] private LayerMask affectedLayers;
    [SerializeField] private Vector2 gravityDir = Vector2.down;
    
    private SpriteRenderer sp;

    // Start is called before the first frame update
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float arrowDir = Vector2.Angle(gravityDir, Vector2.down);
        if (gravityDir.x > 0) arrowDir = 360 - arrowDir;
        sp.material.SetFloat("_ArrowAngle", arrowDir * 3.14f / 180);
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (affectedLayers == (affectedLayers | (1 << other.gameObject.layer))) {
            GravityObject g = other.GetComponent<GravityObject>();
            float oldGravity = g.personalGravity.magnitude;
            Vector2 newGravity = oldGravity * gravityDir.normalized;
            g.personalGravity = newGravity;
        }
    }
}
