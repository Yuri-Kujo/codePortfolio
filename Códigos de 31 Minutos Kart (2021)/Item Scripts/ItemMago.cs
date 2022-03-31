using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMago : MonoBehaviour
{
    public Transform bombSpawner;
    private ItemBoxInteract playerItem;
    private CheckpointCheck cpCheck;

    public GameObject bombInstance;
    public GameObject bombLaunchInstance;
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
        if (!cpCheck.isABot && playerItem.hasItem)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                Instantiate(bombLaunchInstance, bombSpawner.position, bombSpawner.rotation);
                playerItem.UseItem(true);
            }
            else if (Input.GetKeyDown(KeyCode.J))
            {
                GameObject bombLaunchGO = Instantiate(bombLaunchInstance, new Vector3(bombSpawner.position.x, bombSpawner.position.y + 1.3f, bombSpawner.position.z), bombSpawner.rotation);
                bombLaunchGO.GetComponent<Rigidbody>().AddForce(bombSpawner.forward * barrelForce, ForceMode.Impulse);
                bombLaunchGO.GetComponent<Rigidbody>().AddTorque(bombSpawner.right * barrelForce / 2, ForceMode.Impulse);
                playerItem.UseItem(true);
            }
        }

        if (cpCheck.isABot && playerItem.hasItem)
        {
            Instantiate(bombInstance, bombSpawner.position, bombSpawner.rotation);
            playerItem.UseItem(false);
        }
    }
}
