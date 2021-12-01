using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditScroll : MonoBehaviour
{

    [SerializeField] private float scrollSpeed = 3;
    [SerializeField] private GameObject menuReturnButton;

    private bool finishedScroll = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!finishedScroll) {
            transform.position += Vector3.up * scrollSpeed * Time.deltaTime;
            menuReturnButton.transform.position += Vector3.up * scrollSpeed * Time.deltaTime;
        }
        if (menuReturnButton) {
            RectTransform menuButtonTransform = menuReturnButton.GetComponent<RectTransform>();
            if (menuButtonTransform.position.y > 300) {
                finishedScroll = true;
                GetComponent<OpacityFade>().Hide();
            }
        }

    }
}
