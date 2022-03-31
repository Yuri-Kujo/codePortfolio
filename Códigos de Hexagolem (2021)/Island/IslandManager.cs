using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class IslandManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> hexaIslandsCentral, hexaIslandsMid, hexaIslandsBorder;
    [SerializeField] private List<GameObject> hexaIslandsFall;
    [SerializeField] private List<SpriteRenderer> hexaImageCentral, hexaImageMid, hexaImageBorder;
    public float temporizadorCaidaHexaislas;
    public bool activadorCaidaBorder, activadorCaidaMid, activadorCaidaCentral;
    public float borderFallTime, midFallTime, centralFallTime;
    public float elevationPreFall;
    private PhotonView view;
    // Start is called before the first frame update
    void Awake()
    {
        view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            if (temporizadorCaidaHexaislas > 0)
            {
                temporizadorCaidaHexaislas -= Time.deltaTime;
            }

            if (temporizadorCaidaHexaislas <= borderFallTime && !activadorCaidaBorder)
            {
                StartCoroutine(CaidaHexaIsla(hexaIslandsBorder, hexaImageBorder, hexaIslandsBorder.Count, 0));
                activadorCaidaBorder = true;
            }

            if (temporizadorCaidaHexaislas <= midFallTime && !activadorCaidaMid)
            {
                StartCoroutine(CaidaHexaIsla(hexaIslandsMid, hexaImageMid, hexaIslandsMid.Count, 1));
                activadorCaidaMid = true;
            }

            if (temporizadorCaidaHexaislas <= centralFallTime && !activadorCaidaCentral)
            {
                StartCoroutine(CaidaHexaIsla(hexaIslandsCentral, hexaImageCentral, hexaIslandsCentral.Count, 2));
                activadorCaidaCentral = true;
            }
        }
    }

    IEnumerator CaidaHexaIsla(List<GameObject> hexaIsla, List<SpriteRenderer> hexaImage, int islandsCount, int hexaImageList)
    {
        for (int i = 0; i < islandsCount; i++)
        {
            int numeroRandom = Random.Range(0, hexaIsla.Count);
            Debug.Log("Numero Random: " + numeroRandom);
            view.RPC("ChangeIslandColor", RpcTarget.All, (int)numeroRandom, (float)255f, (float)165f, (float)0f, (int)hexaImageList);
            hexaIsla[numeroRandom].GetComponent<Animator>().SetBool("isTemblando", true);
            //hexaImage[numeroRandom].color = new Color(255, 165, 0);
            yield return new WaitForSeconds(10);
            hexaIsla[numeroRandom].GetComponent<Animator>().SetBool("isTemblando", false);
            hexaIsla[numeroRandom].GetComponent<Transform>().LeanMoveLocalY(elevationPreFall, 1);
            yield return new WaitForSeconds(1);
            view.RPC("ChangeIslandColor", RpcTarget.All, (int)numeroRandom, (float)255f, (float)0f, (float)0f, (int)hexaImageList);
            //hexaImage[numeroRandom].color = Color.red;
            //hexaIsla[numeroRandom].GetComponent<Rigidbody>().isKinematic = false;
            hexaIsla[numeroRandom].GetComponent<IslandFall>().isFalling = true;
            hexaIslandsFall.Add(hexaIsla[numeroRandom]);
            hexaIsla.RemoveAt(numeroRandom);
            hexaImage.RemoveAt(numeroRandom);
        }
        yield break;
    }

    [PunRPC]
    void ChangeIslandColor(int numeroIsla, float r, float g, float b, int hexaImageList)
    {
        switch(hexaImageList)
        {
            case 0:
                hexaImageBorder[numeroIsla].color = new Color(r, g, b);
                break;
            case 1:
                hexaImageMid[numeroIsla].color = new Color(r, g, b);
                break;
            case 2:
                hexaImageCentral[numeroIsla].color = new Color(r, g, b);
                break;
            default:
                Debug.LogError("Valor fuera de rango");
                break;
        }
    }
}
