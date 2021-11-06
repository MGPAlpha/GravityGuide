using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Wire : MonoBehaviour
{
    
    private SpriteShapeRenderer re;

    [SerializeField] private bool startOn = false;
    [SerializeField] private Color offColor;
    [SerializeField] private Color onColor;

    private bool on = false;
    
    // Start is called before the first frame update
    void Start()
    {
        re = GetComponent<SpriteShapeRenderer>();
        on = startOn;
        re.color = on ? onColor : offColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPowered(bool setOn) {
        on = setOn;
        re.color = on ? onColor : offColor;
    }

    public void TurnOn() {
        SetPowered(true);
    }

    public void TurnOff() {
        SetPowered(false);
    }

    public void Toggle() {
        SetPowered(!on);
    }
}
