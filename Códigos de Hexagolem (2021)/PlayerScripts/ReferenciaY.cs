using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//------------------------------------------------------------------
// Esto sirve para que el movimiento del jugador sea según el ángulo de la cámara en vez de que sea con respecto a los ejes del mundo en sí.
//------------------------------------------------------------------

public class ReferenciaY : MonoBehaviour
{
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
