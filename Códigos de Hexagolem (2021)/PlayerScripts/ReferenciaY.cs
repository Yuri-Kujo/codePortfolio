using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenciaY : MonoBehaviour
{
    [SerializeField]
    private Transform mainCamera;

    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 yRotation = new Vector3(0, mainCamera.eulerAngles.y, 0);
        transform.eulerAngles = yRotation;
    }
}
