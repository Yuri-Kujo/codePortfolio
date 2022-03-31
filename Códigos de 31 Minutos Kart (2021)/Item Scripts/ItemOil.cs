using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOil : MonoBehaviour
{
    public Transform oilSpawner;
    private ItemBoxInteract playerItem;
    private CheckpointCheck cpCheck;

    public GameObject oilInstance;
    public GameObject oilBarrelInstance;
    public float barrelForce;
    // Start is called before the first frame update
    void Start()
    {
        playerItem = transform.parent.parent.gameObject.GetComponent<ItemBoxInteract>();
        cpCheck = transform.parent.parent.gameObject.GetComponent<CheckpointCheck>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!cpCheck.isABot && playerItem.hasItem)
        {
            if(Input.GetKeyDown(KeyCode.K))
            {
                Instantiate(oilInstance, oilSpawner.position, oilSpawner.rotation);
                playerItem.UseItem(true);
            }
            else if(Input.GetKeyDown(KeyCode.J))
            {
                GameObject barrelGO = Instantiate(oilBarrelInstance, new Vector3(oilSpawner.position.x,oilSpawner.position.y + 1.3f,oilSpawner.position.z), oilSpawner.rotation);
                barrelGO.GetComponent<Rigidbody>().AddForce(oilSpawner.forward * barrelForce,ForceMode.Impulse);
                barrelGO.GetComponent<Rigidbody>().AddTorque(oilSpawner.right * barrelForce / 2, ForceMode.Impulse);
                playerItem.UseItem(true);
            }
        }

        if(cpCheck.isABot && playerItem.hasItem)
        {
            Instantiate(oilInstance, oilSpawner.position, oilSpawner.rotation);
            playerItem.UseItem(false);
        }
    }
}
