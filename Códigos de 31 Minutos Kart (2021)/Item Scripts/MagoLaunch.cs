using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagoLaunch : MonoBehaviour
{
    public GameObject bombInstance;
    private Rigidbody rb;
    private bool canInstance = true;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * 15, ForceMode.Impulse);
        StartCoroutine("ActivateTCol");

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Floor"))
        {
            if(canInstance)
            {
                Instantiate(bombInstance, transform.position, Quaternion.Euler(0, 0, 0));
                canInstance = false;
            }
            Destroy(gameObject);
        }
    }

    IEnumerator ActivateTCol()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<CapsuleCollider>().enabled = true;
        yield break;
    }
}
