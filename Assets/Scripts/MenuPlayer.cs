using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPlayer : MonoBehaviour
{
    [SerializeField] private Color auraColor;
    
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = .2f;
        Aura a = GetComponentInChildren<Aura>();
        a.ActivateAura(true);
        a.SetColor(auraColor);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
