    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CrateZone : MonoBehaviour
{
    [SerializeField] private UnityEvent onTrigger;
    [SerializeField] private UnityEvent onUntrigger;
    [SerializeField] private LayerMask triggerLayers;

    [SerializeField] private bool showGizmos = true;
    
    private int triggerCount = 0;

    private List<GameObject> triggers = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject trigger in triggers) {
            if (triggerLayers != (triggerLayers | (1 << trigger.gameObject.layer))) {
                triggers.Remove(trigger);
                triggerCount--;
                if (triggerCount == 0) {
                    if (onUntrigger.GetPersistentEventCount() > 0) onUntrigger.Invoke();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggerLayers == (triggerLayers | (1 << other.gameObject.layer))) {
            if (triggerCount == 0) {
                if (onTrigger.GetPersistentEventCount() > 0) onTrigger.Invoke();
            }
            triggerCount++;
            triggers.Add(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (triggerLayers == (triggerLayers | (1 << other.gameObject.layer))) {
            triggerCount--;
            if (triggerCount == 0) {
                if (onUntrigger.GetPersistentEventCount() > 0) onUntrigger.Invoke();
            }
            triggers.Remove(other.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        if (showGizmos) {
            if (triggerLayers == (triggerLayers | (1 << LayerMask.NameToLayer("Player")))) {
                Gizmos.DrawIcon(transform.position, "Player Zone");
            }
            CustomGizmos.DrawEventTargets(transform.position, onUntrigger, Color.red);
            CustomGizmos.DrawEventTargets(transform.position, onTrigger, Color.green);
        }
    }
}
