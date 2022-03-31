using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandFall : MonoBehaviour
{
    public bool isFalling = false;
    private float fallingSpeed = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isFalling == true)
        {
            transform.Translate(-Vector3.up * fallingSpeed * Time.deltaTime, Space.World);
            fallingSpeed += Time.deltaTime;
        }
    }
}
