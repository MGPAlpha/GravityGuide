using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisintegrateChildren : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Disintegrate() {
        DissolveDestroy[] children = GetComponentsInChildren<DissolveDestroy>(true);
        foreach (DissolveDestroy child in children) {
            child.Dissolve();
        }
    }
}
