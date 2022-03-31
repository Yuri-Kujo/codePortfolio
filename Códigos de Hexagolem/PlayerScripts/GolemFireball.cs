using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GolemFireball : MonoBehaviour
{
    public GolemController golemController;
    public float pushForce;
    public int attackDamage;
    public float activeTime = 10f;
    private void Update()
    {
        activeTime -= Time.deltaTime;
        if(activeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Tiene mas fuerza vertical que horizontal
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Bolafuego tocó: " + gameObject.name);
            other.transform.Translate(0, 2f, 0, Space.World);
            other.GetComponent<ThirdControllerV2>().estaEnSuelo = false;
            other.GetComponent<AudioSource>().Play();
            other.GetComponent<Rigidbody>().AddForce(transform.up * pushForce, ForceMode.Impulse);
            other.GetComponent<ThirdControllerV2>().hitStun = true;
            PhotonNetwork.Instantiate("FB_Impact", transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        //Quita vida al golem
        else if (other.gameObject.CompareTag("Golem") && other.transform.GetComponent<GolemController>().isMounted)
        {
            Debug.Log("Bolafuego tocó (golem): " + gameObject.name);

            other.transform.GetComponent<AudioSource>().Play();

            if (golemController.view.IsMine)
            {
                other.GetComponent<GolemController>().view.RPC("GolemHPLoss", RpcTarget.All, (float)attackDamage);
            }
            PhotonNetwork.Instantiate("FB_Impact", transform.position, Quaternion.identity);
            PhotonNetwork.Destroy(gameObject);
        }
        else
        {
            PhotonNetwork.Instantiate("FB_Impact", transform.position, Quaternion.identity);
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
