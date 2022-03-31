using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemIceBreath : MonoBehaviour
{
    public float slowMultiplier;
    public float slowDuration;

    private void OnTriggerEnter(Collider collision)
    {
        //Tiene mas fuerza vertical que horizontal
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.GetComponent<ThirdControllerV2>().StartCoroutine("Slowness", slowMultiplier);
        }
        //Quita vida al golem
        else if (collision.gameObject.CompareTag("Golem") && collision.transform.GetComponent<GolemController>().isMounted)
        {
            collision.GetComponent<GolemController>().StartCoroutine("Slowness", slowMultiplier);
        }
    }
}
