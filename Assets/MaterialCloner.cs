using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialCloner : MonoBehaviour
{
    private void Awake()
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        renderer.material = new Material(renderer.material);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
