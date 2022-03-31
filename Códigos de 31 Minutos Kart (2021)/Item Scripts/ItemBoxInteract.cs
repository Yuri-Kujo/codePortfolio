using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBoxInteract : MonoBehaviour
{
    public bool hasItem = false;

    public GameObject[] itemGameObjects;
    public Sprite[] itemSprites;
    public Image yourSprite;

    public Animator itemUIAnim;
    public Animator itemUIScroll;

    public int index;

    private void Start()
    {
        if(!GetComponent<CheckpointCheck>().isABot)
        {
            yourSprite = GameObject.Find("YOUR_ITEM").GetComponent<Image>();
            itemUIAnim = GameObject.Find("ItemHUD").GetComponent<Animator>();
            itemUIScroll = GameObject.Find("Items").GetComponent<Animator>();
        }
    }
    private void Update()
    {
        //Desechar items sin funcionamiento, quitar despues de completar todo
        if(hasItem && Input.GetKeyDown(KeyCode.P))
        {
            hasItem = false;
            itemGameObjects[index].SetActive(false);
            if(!GetComponent<CheckpointCheck>().isABot)
            {
                itemUIAnim.SetBool("ItemIn", false);
                itemUIScroll.SetBool("Scroll", false);
            }
        }
    }
    private IEnumerator OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "ItemBox")
        {
            other.gameObject.GetComponent<SphereCollider>().enabled = false;
            other.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            other.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            other.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            //Corrutina Ruleta
            StartCoroutine(GetItem());
            if(!gameObject.GetComponent<CheckpointCheck>().isABot)
            {
                itemUIAnim.SetBool("ItemIn", true);
                itemUIScroll.SetBool("Scroll", true);
            }
            //Re-Enable Box
            yield return new WaitForSeconds(1f);
            other.gameObject.GetComponent<SphereCollider>().enabled = true;
            other.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
            other.transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().enabled = true;
            other.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<MeshRenderer>().enabled = true;
        }
        yield break;
    }
    public IEnumerator GetItem()
    {
        if(!hasItem)
        {
            index = Random.Range(0, itemGameObjects.Length);
            if (!gameObject.GetComponent<CheckpointCheck>().isABot)
            {
                yourSprite.sprite = itemSprites[index];
            }
            yield return new WaitForSeconds(3f);
            itemGameObjects[index].SetActive(true);
            hasItem = true;
            
        }
        yield break;
    }

    public void UseItem(bool isAPlayer)
    {
        if(isAPlayer)
        {
            hasItem = false;
            itemUIAnim.SetBool("ItemIn", false);
            itemUIScroll.SetBool("Scroll", false);
            itemGameObjects[index].SetActive(false);
        }
        else
        {
            hasItem = false;
            itemGameObjects[index].SetActive(false);
        }
        
    }
}
