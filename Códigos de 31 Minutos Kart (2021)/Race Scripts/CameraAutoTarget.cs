using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAutoTarget : MonoBehaviour
{
    CinemachineVirtualCamera mainCam;
    private void Awake()
    {
        mainCam = GetComponent<CinemachineVirtualCamera>();
    }
    // Start is called before the first frame update
    void Start()
    {
        mainCam.Follow = GameObject.FindGameObjectWithTag("Player").transform;
        mainCam.LookAt = GameObject.FindGameObjectWithTag("BodyPlayer").transform;
    }
}
