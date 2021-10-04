using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightSprite : MonoBehaviour
{
    [SerializeField] private bool highlightActive = false;
    [SerializeField] private Color highlightColor = Color.white;

    private SpriteRenderer sp;
    
    public void SetHighlight(bool active) {
        GetComponent<SpriteRenderer>().material.SetInt("_Outline", active? 1 : 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
