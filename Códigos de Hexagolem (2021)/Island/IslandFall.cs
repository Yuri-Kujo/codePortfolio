using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandFall : MonoBehaviour
{
    public bool isFalling = false;
    private float fallingSpeed = 5;

    void Update()
    {
        if (isFalling == true)
        {
            transform.Translate(-Vector3.up * fallingSpeed * Time.deltaTime, Space.World);
            fallingSpeed += Time.deltaTime;
        }
    }
}
