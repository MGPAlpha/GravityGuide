using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aura : MonoBehaviour
{
    List<GravityObject> currentGravityObjects = new List<GravityObject>();

    [SerializeField] private float fullSize;
    [SerializeField] private float inactiveSizeRatio;
    [SerializeField] private float activationTime = .5f;

    private float activation = 0;
    private bool auraActive = false;

    private Color color;


    [SerializeField] private LayerMask affectedLayers;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("update");
        activation += Time.unscaledDeltaTime * (auraActive ? 1 : -1);
        activation = Mathf.Clamp(activation, 0, activationTime);
        float percentActive = activation / activationTime;
        transform.localScale = new Vector3(1,1,0) * Mathf.SmoothStep(inactiveSizeRatio * fullSize, fullSize, percentActive);
        GetComponent<SpriteRenderer>().color = new Color(color.r,color.g,color.b, Mathf.SmoothStep(0,1,percentActive)*color.a);
    }

    public void AlterGravity(Vector2 newGravity) {
        foreach (GravityObject g in currentGravityObjects) {
            if (!g) continue;
            g.personalGravity = g.personalGravity.magnitude * newGravity;
        }
    }

    public void ActivateAura(bool active) {
        auraActive = active;
    }

    public void SetColor(Color c) {
        color = c;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (affectedLayers == (affectedLayers | (1 << other.gameObject.layer))) {
            currentGravityObjects.Add(other.GetComponent<GravityObject>());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (affectedLayers == (affectedLayers | (1 << other.gameObject.layer))) {
            currentGravityObjects.Remove(other.GetComponent<GravityObject>());
        }
    }

    public void SetRadius(float radius) {
        fullSize =  2 * radius;
    }
}
