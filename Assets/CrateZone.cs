using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CrateZone : MonoBehaviour
{
    [SerializeField] private UnityEvent onTrigger;
    [SerializeField] private UnityEvent onUntrigger;
    [SerializeField] private LayerMask triggerLayers;
    
    private int triggerCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggerLayers == (triggerLayers | (1 << other.gameObject.layer))) {
            if (triggerCount == 0) {
                if (onTrigger.GetPersistentEventCount() > 0) onTrigger.Invoke();
            }
            triggerCount++;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (triggerLayers == (triggerLayers | (1 << other.gameObject.layer))) {
            triggerCount--;
            if (triggerCount == 0) {
                if (onUntrigger.GetPersistentEventCount() > 0) onUntrigger.Invoke();
            }
        }
    }
}
