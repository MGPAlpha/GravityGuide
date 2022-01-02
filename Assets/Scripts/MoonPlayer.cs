using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoonPlayer : Player
{
    [SerializeField] private UnityEvent onActivateGravity;

    public override void ActivateGravity(bool self) {
        base.ActivateGravity(self);
        if (onActivateGravity.GetPersistentEventCount() > 0) onActivateGravity.Invoke();
    }
}
