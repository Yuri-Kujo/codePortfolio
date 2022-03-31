using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterMenu : MonoBehaviour
{
    public NetworkManager netManager;

    [Header("Paneles")]
    public GameObject[] paneles;

    public void Start()
    {
        netManager = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetworkManager>();
    }

    public void Update()
    {
        if(netManager.conectadoAlServidor == true)
        {
            ConectadoAlServidor();
            netManager.conectadoAlServidor = false;
        }
    }

    public void ConectadoAlServidor()
    {
        paneles[0].SetActive(false);
        paneles[2].SetActive(true);
        paneles[1].SetActive(true);
    }

    public void JugarPartida()
    {
        netManager.Conectado();
    }
}
