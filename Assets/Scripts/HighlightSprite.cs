using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightSprite : MonoBehaviour
{
    [SerializeField] private bool highlightActive = false;
    [SerializeField] private Color highlightColor = Color.white;

    private SpriteRenderer sp;

    private Animator anim;
    
    public void SetHighlight(bool active) {
        sp.material.SetInt("_Outline", active? 1 : 0);
        if (anim) {
            anim.SetBool("Open", active);
            anim.SetFloat("speedMul", active ? 1 : -1);
            Debug.Log(anim.GetFloat("speedMul"));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent<Animator>(out anim);
        sp = GetComponent<SpriteRenderer>();
        if (anim) 
            anim.SetFloat("speedMul", highlightActive ? 1 : -1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
