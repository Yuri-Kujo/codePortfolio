using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCalcetin : MonoBehaviour
{
    private ItemBoxInteract playerItem;
    private CheckpointCheck cpCheck;
    private AudioSource bgm;
    public GameObject socksGO;
    public float spinSpeed;
    // Start is called before the first frame update
    void Start()
    {
        playerItem = transform.parent.parent.gameObject.GetComponent<ItemBoxInteract>();
        cpCheck = transform.parent.parent.gameObject.GetComponent<CheckpointCheck>();
        bgm = GameObject.Find("AudioManager").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!cpCheck.isABot && playerItem.hasItem)
        {
            if (Input.GetKeyDown(KeyCode.K)||Input.GetKeyDown(KeyCode.J))
            {
                socksGO.SetActive(true);
                bgm.Pause();
                socksGO.GetComponent<SockActive>().StartCoroutine("Invincibility");
                socksGO.transform.rotation = Quaternion.Euler(Vector3.zero);
                playerItem.UseItem(true);
            }
        }
        if (cpCheck.isABot && playerItem.hasItem)
        {
            socksGO.SetActive(true);
            socksGO.GetComponent<SockActive>().StartCoroutine("Invincibility");
            socksGO.transform.rotation = Quaternion.Euler(Vector3.zero);
            playerItem.UseItem(false);
        }
        
    }
}
