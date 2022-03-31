using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//------------------------------------------------------------------
// Si la hitbox de ataque golpea a un explorador, este es empujado hacia la dirección del golpe.
// Si golpea a un golem, este pierde una cantidad de vida.
//------------------------------------------------------------------
public class GolemAttack : MonoBehaviour
{
    private float pushForce;
    private int attackDamage;
     
    private void Awake()
    {
        pushForce = GetComponentInParent<GolemController>().pushForce;
        attackDamage = GetComponentInParent<GolemController>().attackDamage;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Has pegado a: " + gameObject.name);
            other.transform.Translate(0, 2f, 0, Space.World);
            other.GetComponent<ThirdControllerV2>().estaEnSuelo = false;
            other.GetComponent<AudioSource>().Play();
            other.GetComponent<Rigidbody>().AddForce(transform.forward * pushForce, ForceMode.Impulse);
            other.GetComponent<Rigidbody>().AddForce(transform.up * pushForce,ForceMode.Impulse);
            other.GetComponent<ThirdControllerV2>().hitStun = true;
        }

        if (other.gameObject.CompareTag("Golem") && other.GetComponent<GolemController>().isMounted)
        {
            Debug.Log("Has pegado a: " + gameObject.name);


            if(GetComponentInParent<GolemController>().view.IsMine)
            {
                other.GetComponent<AudioSource>().Play();
                other.GetComponent<GolemController>().view.RPC("GolemHPLoss", Photon.Pun.RpcTarget.All, (float)attackDamage);
            }
        }
    }
}
