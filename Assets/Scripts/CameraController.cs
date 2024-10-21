using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{

    [SerializeField]
    [Range(0,10)]
    private float cameraStiffness;

    private CinemachineCameraOffset cameraOffset;

    // Start is called before the first frame update
    void Start()
    {
        cameraOffset = GetComponent<CinemachineCameraOffset>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 offset = -(transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition)) / (cameraStiffness + 2);
        // transform.position = playerTransform.position - (playerTransform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition)) / (cameraStiffness + 2);
        // transform.position = Vector3.ProjectOnPlane(transform.position, Vector3.back);
        // transform.position += Vector3.forward * defaultCameraZ;
        cameraOffset.m_Offset = offset;
    }
}