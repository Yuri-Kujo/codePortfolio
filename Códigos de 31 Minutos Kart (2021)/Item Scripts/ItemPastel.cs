using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPastel : MonoBehaviour
{
    private ItemBoxInteract playerItemScript;
    private CheckpointCheck cpCheck;
    private GameObject pastelUI;

    void Start()
    {
        playerItemScript = transform.parent.parent.GetComponent<ItemBoxInteract>();
        cpCheck = transform.parent.parent.gameObject.GetComponent<CheckpointCheck>();
        pastelUI = GameObject.Find("Pastelazo");
    }
    void Update()
    {
        if (!cpCheck.isABot && playerItemScript.hasItem)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                playerItemScript.UseItem(true);
            }
            else if (Input.GetKeyDown(KeyCode.J))
            {
                playerItemScript.UseItem(true);
            }
        }

        if (cpCheck.isABot && playerItemScript.hasItem)
        {
            if(!pastelUI.transform.GetChild(0).gameObject.active)
            {
                pastelUI.transform.GetChild(0).gameObject.SetActive(true);
            }
            playerItemScript.UseItem(false);
        }
    }
}
