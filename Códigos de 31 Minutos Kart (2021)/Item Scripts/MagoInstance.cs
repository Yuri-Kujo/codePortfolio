using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagoInstance : MonoBehaviour
{
    public bool isTheExplosion;
    public float explosionTimer = 1f;
    private void OnTriggerEnter(Collider other)
    {
        if(isTheExplosion && (other.transform.parent.parent.CompareTag("Player") || other.transform.parent.parent.CompareTag("AIPlayer")))
        {
            other.transform.parent.parent.GetComponent<CheckpointCheck>().StartCoroutine("Stun");
        }
    }
    private void Update()
    {
        if (isTheExplosion)
        {
            explosionTimer -= Time.deltaTime /2;
            if(explosionTimer <= 0)
            {
                Destroy(transform.parent.gameObject);
            }
        }
        else
        {
            explosionTimer -= Time.deltaTime / 3;
            if(explosionTimer <= 0)
            {
                transform.GetChild(0).gameObject.SetActive(true);
                GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }
}
