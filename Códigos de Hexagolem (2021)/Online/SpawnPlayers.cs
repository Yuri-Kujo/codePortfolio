using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//------------------------------------------------------------------
// Al iniciar la partida, se instancia al jugador en una ubicación aleatoria del mapa en X y Z.
//------------------------------------------------------------------
public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;

    public float minX, maxX, minZ, maxZ;

    private void Start()
    {
        Vector2 randomPosition = new Vector3(Random.Range(minX, maxX), 3.5f, Random.Range(minZ, maxZ));
        PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);
    }
}
