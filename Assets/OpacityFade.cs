using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OpacityFade : MonoBehaviour
{
    [SerializeField] private float fadeDur = .5f;
    [SerializeField] private bool visible = false;

    private float fade = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        if (visible) fade = fadeDur;
    }

    public void Hide() {
        visible = false;
    }

    public void Show() {
        visible = true;
    }

    public void Toggle() {
        visible = !visible;
    }

    // Update is called once per frame
    void Update()
    {
        fade += Time.deltaTime * (visible ? 1 : -1);
        fade = Mathf.Clamp(fade, 0, fadeDur);

        SpriteRenderer[] children = GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer child in children) {
            Color newColor = child.color;
            newColor.a = Mathf.Lerp(0,1,fade / fadeDur);
            child.color = newColor;
        }

        TextMeshPro[] tmChildren = GetComponentsInChildren<TextMeshPro>();
        foreach(TextMeshPro child in tmChildren) {
            Color newColor = child.color;
            newColor.a = Mathf.Lerp(0,1,fade / fadeDur);
            child.color = newColor;
        }
    }
}
