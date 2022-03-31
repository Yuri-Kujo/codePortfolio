using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GolemWall : MonoBehaviour
{
    public float wallUpTime = 5f;
    // Update is called once per frame
    void Update()
    {
        wallUpTime -= Time.deltaTime;

        if(wallUpTime <= 0)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
