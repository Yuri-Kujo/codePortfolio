using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//------------------------------------------------------------------
//La pared generada por el golem de piedra que desaparece después del tiempo indicado en wallUpTime
//------------------------------------------------------------------
public class GolemWall : MonoBehaviour
{
    public float wallUpTime = 5f;

    void Update()
    {
        wallUpTime -= Time.deltaTime;

        if(wallUpTime <= 0)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
