using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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
        Debug.Log("Tocado objeto del tag: " + other.tag);
        if (other.CompareTag("IslandHitbox"))
        {
            view.RPC("IslandRemoval", RpcTarget.All, other.GetComponentInParent<PhotonView>().ViewID);
        }

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
        if (other.CompareTag("Golem"))
        {
            if(other.GetComponent<GolemController>().playerMounting != null)
            {
                other.GetComponent<GolemController>().view.RPC("DismountGolem", RpcTarget.All, (bool)false, other.GetComponent<GolemController>().view.ViewID);
            }
            if(activeGolems.Count <= maxGolems)
            {
                int r = Random.Range(0, activeIslands.Count);
                int randomX = Random.Range(-3, 3);
                int randomZ = Random.Range(-3, 3);
                other.transform.position = activeIslands[r].transform.position + new Vector3(randomX, 1, randomZ);
            }
            else
            {
                activeGolems.Remove(other.gameObject);
                other.gameObject.SetActive(false);
            }
        }
    }
    [PunRPC]
    void IslandRemoval(int viewID)
    {
        activeIslands.Remove(PhotonView.Find(viewID).gameObject);
        PhotonView.Find(viewID).gameObject.SetActive(false);
        maxGolems = activeIslands.Count / 2 + 1;
        //Debug.Log("New Golem Max is: " + maxGolems);
    }
}
