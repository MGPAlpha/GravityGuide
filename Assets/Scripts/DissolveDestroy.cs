using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveDestroy : MonoBehaviour
{

    [SerializeField] private float dissolveTime = 1;

    private bool dissolving = false;
    private float dissolveProgress = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dissolving) {
            dissolveProgress += Time.deltaTime;
            GetComponent<Renderer>().material.SetFloat("_Dissolve", dissolveProgress / dissolveTime);
            if (dissolveProgress >= dissolveTime) {
                Destroy(gameObject);
            }
        }
    }

    public void Dissolve() {
        dissolving = true;
        gameObject.layer = LayerMask.NameToLayer("Dissolving");
    }
}
