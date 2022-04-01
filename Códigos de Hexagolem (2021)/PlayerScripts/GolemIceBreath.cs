using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemIceBreath : MonoBehaviour
{
    public float slowMultiplier;
    public float slowDuration;

    private void OnTriggerEnter(Collider collision)
    {
        //Ralentiza al explorador si no está montado en golem.
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.GetComponent<ThirdControllerV2>().StartCoroutine("Slowness", slowMultiplier);
        }
        //Ralentiza al golem si el jugador está montado.
        else if (collision.gameObject.CompareTag("Golem") && collision.transform.GetComponent<GolemController>().isMounted)
        {
            collision.GetComponent<GolemController>().StartCoroutine("Slowness", slowMultiplier);
        }
    }
}
