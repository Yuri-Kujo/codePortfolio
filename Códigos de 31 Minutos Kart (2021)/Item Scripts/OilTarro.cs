using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilTarro : MonoBehaviour
{
    public GameObject oilInstance;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * 15, ForceMode.Impulse);

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Floor"))
        {
            Instantiate(oilInstance, transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(gameObject);
        }
    }
}
