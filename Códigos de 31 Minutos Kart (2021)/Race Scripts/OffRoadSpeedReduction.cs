using KartGame.KartSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffRoadSpeedReduction : MonoBehaviour
{
    public float originalMaxSpeed;
    public float offRoadStat;
    public bool modoTurbo;
    private ArcadeKart arcadeKart;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Awake()
    {
        arcadeKart = GetComponent<ArcadeKart>();
        rb = GetComponent<Rigidbody>();
        originalMaxSpeed = arcadeKart.baseStats.TopSpeed;
    }

    private void OnTriggerStay(Collider other)
    {
        if (modoTurbo)
            return;
        if (other.gameObject.CompareTag("NoRaceCircuit") && arcadeKart.GroundPercent > 0.8f)
        {
            if(Input.GetAxis("Accelerate") > 0)
            {
                rb.AddForce(-transform.forward * ((rb.velocity.magnitude/2) * (1-offRoadStat)),ForceMode.Acceleration);
            }
            if(Input.GetAxis("Brake") > 0)
            {
                rb.AddForce(transform.forward * ((rb.velocity.magnitude/2) * (1-offRoadStat)), ForceMode.Acceleration);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("NoRaceCircuit"))
        {
            arcadeKart.baseStats.TopSpeed = originalMaxSpeed;
        }
    }

}
