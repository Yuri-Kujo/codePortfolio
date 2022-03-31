using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PastelDuration : MonoBehaviour
{
    public float duration = 8f;

    // Update is called once per frame
    void Update()
    {
        if(duration > 0)
        {
            duration -= Time.deltaTime;
        }
        if(duration <= 0)
        {
            duration = 8f;
            gameObject.SetActive(false);
        }
    }
}
