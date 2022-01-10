using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Image))]
public class SpinnyImage : MonoBehaviour
{
    [SerializeField] private float spinSpeed = 3;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }



    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0,0,spinSpeed * Time.deltaTime));
    }
}
