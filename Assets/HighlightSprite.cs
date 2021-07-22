using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightSprite : MonoBehaviour
{
    [SerializeField] private bool highlightActive = false;
    [SerializeField] private Color highlightColor = Color.white;

    private SpriteRenderer sp;
    
    public void SetHighlight(bool active) {
        sp.color = active ? highlightColor : Color.white;
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
