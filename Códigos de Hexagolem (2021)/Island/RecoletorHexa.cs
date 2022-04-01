using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

//------------------------------------------------------------------
// Script del collider gigante que hay bajo el mapa.
// Este se encarga de hacer respawnear los golems, destruir los trozos de isla que caen, y otrorgar el estado de derrota a los jugadores que caen.
//------------------------------------------------------------------

public class RecoletorHexa : MonoBehaviour
{
    public List<GameObject> activeIslands;
    public List<GameObject> activeGolems;
    public int maxGolems;
    public GameManager gm;

    PhotonView view;
    private void Awake()
    {
        view = GetComponent<PhotonView>();
    }
    private void Start()
    {
        activeIslands.AddRange(GameObject.FindGameObjectsWithTag("Island"));
        activeGolems.AddRange(GameObject.FindGameObjectsWithTag("Golem"));
    }
    private void OnTriggerEnter(Collider other)
    {
        // La isla que toque este collider es removida del juego mediante esta función RPC.
        Debug.Log("Tocado objeto del tag: " + other.tag);
        if (other.CompareTag("IslandHitbox"))
        {
            view.RPC("IslandRemoval", RpcTarget.All, other.GetComponentInParent<PhotonView>().ViewID);
        }

        // El jugador pierde al caer del mapa.
        if (other.CompareTag("Player"))
        {
            if(other.GetComponentInChildren<SkinnedMeshRenderer>().enabled == true)
            {
                other.GetComponent<ThirdControllerV2>().canMove = false;
                other.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
                other.GetComponent<Rigidbody>().isKinematic = true;
                gm.playersLost.Add(other.gameObject);
                gm.SetPlaceInLost();
                gm.playersInGame.Remove(other.gameObject);
                other.transform.GetChild(8).gameObject.SetActive(false);
                other.transform.GetChild(6).GetChild(0).gameObject.SetActive(false);
                other.GetComponent<PlayerUI>().placeinLost = gm.seterPlayerLostPlace;
                other.GetComponent<PlayerUI>().lostPlaceText.text = "You placed " + other.GetComponent<PlayerUI>().placeinLost;
                other.GetComponent<PlayerUI>().lostPlaceText2.text = "You placed " + other.GetComponent<PlayerUI>().placeinLost;
                other.GetComponent<ThirdControllerV2>().isLost = true;
            }
            
        }
        //Si un explorador está montado al golem, el explorador es automáticamente expulsado del golem.
        if (other.CompareTag("Golem"))
        {
            if(other.GetComponent<GolemController>().playerMounting != null)
            {
                other.GetComponent<GolemController>().view.RPC("DismountGolem", RpcTarget.All, (bool)false, other.GetComponent<GolemController>().view.ViewID);
            }
            // Si el número de golems es menor o igual a la cantidad de golems máximos en partida, se permite su respawneo en un trozo de isla aleatorio.
            if(activeGolems.Count <= maxGolems)
            {
                int r = Random.Range(0, activeIslands.Count);
                int randomX = Random.Range(-3, 3);
                int randomZ = Random.Range(-3, 3);
                other.transform.position = activeIslands[r].transform.position + new Vector3(randomX, 1, randomZ);
            }
            // De lo contrario, el golem es desactivado.
            else
            {
                activeGolems.Remove(other.gameObject);
                other.gameObject.SetActive(false);
            }
        }
    }

    //Desactiva el trozo de isla, y es removida de la lista de trozos de isla activos.
    [PunRPC]
    void IslandRemoval(int viewID)
    {
        activeIslands.Remove(PhotonView.Find(viewID).gameObject);
        PhotonView.Find(viewID).gameObject.SetActive(false);
        //La cantidad de golems que puede haber en la partida depende de la cantidad de islas activas en ese momento.
        maxGolems = activeIslands.Count / 2 + 1;
    }
}
