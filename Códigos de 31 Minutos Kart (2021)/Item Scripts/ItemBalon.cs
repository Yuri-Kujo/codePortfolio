using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBalon : MonoBehaviour
{
    public Transform spawnerForward;
    public Transform spawnerBackward;

    private ItemBoxInteract playerItem;
    private CheckpointCheck cpCheck;

    public GameObject balonInstance;

    Vector3 additionalDistance = new Vector3(0,.75f,0);
    // Start is called before the first frame update
    void Start()
    {
        playerItem = transform.parent.parent.gameObject.GetComponent<ItemBoxInteract>();
        cpCheck = transform.parent.parent.gameObject.GetComponent<CheckpointCheck>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!cpCheck.isABot && playerItem.hasItem)
        {
            if(Input.GetKeyDown(KeyCode.K))
            {
                Instantiate(balonInstance, spawnerBackward.position - additionalDistance, spawnerBackward.rotation);
                playerItem.UseItem(true);
            }
            else if(Input.GetKeyDown(KeyCode.J))
            {
                Instantiate(balonInstance, spawnerForward.position + additionalDistance, spawnerForward.rotation);
                playerItem.UseItem(true);
            }
            
        }

        if (cpCheck.isABot && playerItem.hasItem)
        {
            if(cpCheck.kartPosition < 1)
            {
                Instantiate(balonInstance, spawnerForward.position +additionalDistance, spawnerForward.rotation);
                playerItem.UseItem(false);
            }
            else if(cpCheck.kartPosition == 1)
            {
                Instantiate(balonInstance, spawnerBackward.position+additionalDistance, spawnerBackward.rotation);
                playerItem.UseItem(false);
            }
            
        }
    }
}
