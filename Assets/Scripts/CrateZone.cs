using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CrateZone : MonoBehaviour
{
    [SerializeField] private UnityEvent onTrigger;
    [SerializeField] private UnityEvent onUntrigger;
    [SerializeField] private UnityEvent onToggle;
    [SerializeField] private LayerMask triggerLayers;

    [SerializeField] private bool singleUse = false;

    private bool usedOnce = false;

    [SerializeField] private bool showGizmos = true;
    
    private int triggerCount = 0;

    private List<GameObject> triggers = new List<GameObject>();

    [SerializeField] private float borderFillSpeed = 1f;

    private float borderFill = .5f;
    private float borderFillTarget = .5f;

    private SpriteRenderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent<SpriteRenderer>(out renderer);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject trigger in triggers) {
            if (triggerLayers != (triggerLayers | (1 << trigger.gameObject.layer))) {
                triggers.Remove(trigger);
                triggerCount--;
                if (triggerCount == 0) {
                    if (!singleUse || !usedOnce) {
                        if (onUntrigger.GetPersistentEventCount() > 0) onUntrigger.Invoke();
                        if (onToggle.GetPersistentEventCount() > 0) onToggle.Invoke();
                    }
                    borderFillTarget = .5f;
                    usedOnce = true;
                }
            }
        }
        borderFill = Mathf.MoveTowards(borderFill, borderFillTarget, borderFillSpeed * Time.deltaTime);
        if (renderer) renderer.material.SetFloat("_BorderFill", borderFill);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggerLayers == (triggerLayers | (1 << other.gameObject.layer))) {
            if (triggerCount == 0) {
                if (!singleUse || !usedOnce) {
                    if (onTrigger.GetPersistentEventCount() > 0) onTrigger.Invoke();
                    if (onToggle.GetPersistentEventCount() > 0) onToggle.Invoke();
                }
                borderFillTarget = 1;
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
                if (!singleUse || !usedOnce) {
                    if (onUntrigger.GetPersistentEventCount() > 0) onUntrigger.Invoke();
                    if (onToggle.GetPersistentEventCount() > 0) onToggle.Invoke();
                }
                borderFillTarget = .5f;
                usedOnce = true;
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
            CustomGizmos.DrawEventTargets(transform.position, onToggle, Color.blue);
        }
    }
}
