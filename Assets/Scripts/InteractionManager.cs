using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    List<Interactible> inRange;

    Interactible target = null;

    [SerializeField] private LayerMask interactibleLayers;

    void Start()
    {
        inRange = new List<Interactible>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) | interactibleLayers) == interactibleLayers) {
            Interactible newInter = other.GetComponent<Interactible>();
            inRange.Add(newInter);
            if ((!target || newInter.GetPriority() >= target.GetPriority()) && newInter.CanTarget()) {
                if (target) target.Detarget();
                target = newInter;
                target.Target();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) | interactibleLayers) == interactibleLayers) {
            Interactible oldInter = other.GetComponent<Interactible>();
            inRange.Remove(oldInter);
            if (target == oldInter) {
                target.Detarget();
                FindTarget();
            }
        }
    }

    private void FindTarget() {
        if (inRange.Count <= 0) target = null;
        else {
            target = null;
            foreach (Interactible inter in inRange) {
                if ((!target || inter.GetPriority() > target.GetPriority()) && inter.CanTarget()) target = inter;
            }
            if (target) target.Target();
        }
    }

    public void Interact() {
        if (target) {
            bool keepTarget = target.Interact();
            if (!keepTarget) {
                target.Detarget();
                FindTarget();
            }
        }
    }
}
