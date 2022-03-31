using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

//------------------------------------------------------------------
// Este script fue un intento de resolver un bug que implicaba que dos jugadores se quedaban atascados cuando montaban a la vez un mismo golem.
// En general, funcionó bastante bien.
//------------------------------------------------------------------

public class DoubleDismounter : MonoBehaviour
{
    [SerializeField] private ThirdControllerV2 rapiController;
    [SerializeField] private GolemController golemController;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Golem"))
        {
            golemController = other.GetComponent<GolemController>();
        }
        if(other.CompareTag("Dismounter"))
        {
            ThirdControllerV2 rapiController2 = other.GetComponent<DoubleDismounter>().rapiController;
            if (rapiController.enabled == false)
            {
                //Desactivar UI
                golemController.isMounted = false;
                golemController.hpBar.fillAmount = 0;
                golemController.hpBar = null;
                golemController.mouse2Image.color = Color.black;
                golemController.mouse2Image = null;
                golemController.mouse1Cooldown = null;
                golemController.mouse2Cooldown = null;
                golemController.mouse1Image.sprite = rapiController.mountSprite;
                golemController.mouse1Image = null;
                golemController.playerMounting = null;
                rapiController.hpBar.gameObject.SetActive(false);
                //Anular dueño de Golem
                golemController.view.TransferOwnership(golemController.view.ViewID);
                //Anular parents, activar controladores
                CinemachineFreeLook vCam = rapiController.GetComponentInChildren<CinemachineFreeLook>();
                vCam.Follow = rapiController.transform;
                vCam.LookAt = rapiController.transform;
                vCam.m_Orbits[0].m_Height = 7.63f;
                vCam.m_Orbits[0].m_Radius = 4.4f;
                vCam.m_Orbits[1].m_Radius = 14.67f;
                vCam.m_Orbits[2].m_Height = .16f;
                vCam.m_Orbits[2].m_Radius = 4.54f;
                rapiController.canRaycast = true;
                rapiController.GetComponent<Rigidbody>().isKinematic = false;
                rapiController.transform.parent = null;
                rapiController.enabled = true;
                rapiController.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)) * 13, ForceMode.Impulse);
                rapiController.GetComponent<CapsuleCollider>().enabled = true;
                rapiController.GetComponentInChildren<Animator>().SetBool("Mounted", false);
                rapiController2.transform.parent = null;
                rapiController2.enabled = true;
                golemController = null;
            }
        }
    }
}
