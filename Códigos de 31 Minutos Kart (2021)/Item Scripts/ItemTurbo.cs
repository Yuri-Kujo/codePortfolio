using KartGame.KartSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTurbo : MonoBehaviour
{
    public float turboPower;
    private ItemBoxInteract playerItem;
    private CheckpointCheck cpCheck;
    private Rigidbody rb;
    public GameObject turboVFX;
    // Start is called before the first frame update
    void Start()
    {
        playerItem = transform.parent.parent.gameObject.GetComponent<ItemBoxInteract>();
        cpCheck = transform.parent.parent.gameObject.GetComponent<CheckpointCheck>();
        rb = transform.parent.parent.gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!cpCheck.isABot && playerItem.hasItem && (Input.GetKeyDown(KeyCode.K)||Input.GetKeyDown(KeyCode.J)))
        {
            StartCoroutine("TurboActive");
            playerItem.hasItem = false;
            playerItem.itemUIAnim.SetBool("ItemIn", false);
            playerItem.itemUIScroll.SetBool("Scroll", false);
        }

        if(cpCheck.isABot && playerItem.hasItem)
        {
            StartCoroutine("TurboActiveBot");
            playerItem.UseItem(false);
        }
    }

    IEnumerator TurboActive()
    {
        turboVFX.SetActive(true);
        transform.parent.parent.gameObject.GetComponent<OffRoadSpeedReduction>().modoTurbo = true;
        rb.AddForce(transform.parent.parent.forward * turboPower, ForceMode.VelocityChange);
        yield return new WaitForSeconds(2f);
        turboVFX.SetActive(false);
        if(transform.parent.parent.gameObject.GetComponent<ArcadeKart>().AirPercent <.75f)
        {
            rb.AddForce(-transform.parent.parent.forward * (rb.velocity.magnitude * 0.5f), ForceMode.VelocityChange);

        }
        else
        {
            rb.AddForce(-transform.parent.parent.forward * (rb.velocity.magnitude * 0.25f), ForceMode.VelocityChange);
        }
        transform.parent.parent.gameObject.GetComponent<OffRoadSpeedReduction>().modoTurbo = false;
        playerItem.itemGameObjects[2].SetActive(false);
        yield break;
    }
    IEnumerator TurboActiveBot()
    {
        turboVFX.SetActive(true);
        transform.parent.parent.gameObject.GetComponent<OffRoadSpeedReduction>().modoTurbo = true;
        rb.AddForce(transform.parent.parent.forward * turboPower, ForceMode.VelocityChange);
        yield return new WaitForSeconds(2f);
        turboVFX.SetActive(false);
        if (transform.parent.parent.gameObject.GetComponent<ArcadeKart>().AirPercent < .75f)
        {
            rb.AddForce(-transform.parent.parent.forward * (rb.velocity.magnitude * 0.5f), ForceMode.VelocityChange);

        }
        else
        {
            rb.AddForce(-transform.parent.parent.forward * (rb.velocity.magnitude * 0.25f), ForceMode.VelocityChange);
        }
        transform.parent.parent.gameObject.GetComponent<OffRoadSpeedReduction>().modoTurbo = false;
        playerItem.itemGameObjects[2].SetActive(false);
        yield break;
    }
}
