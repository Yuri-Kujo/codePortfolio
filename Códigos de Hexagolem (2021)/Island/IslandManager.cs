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

    // El cliente del host es quien da la orden de que islas se caen al resto de los jugadores.
    // Al cabo de un tiempo empiezan a caer los trozos de isla dentro de la lista hexaIslandsBorder (las islas al borde del mapa) de forma aleatoria.
    // Después de otro tiempo, se suma a la caída los trozos de la lista hexaIslandsMid, y lo mismo pasa más tarde con hexaIslandsCentral.
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

    //Se elige aleatoriamente el trozo de isla que caerá. Esta tiembla por 10 segundos y luego cae.
    //Al final, es removida de la lista de trozos de isla activos en el mapa.
    IEnumerator CaidaHexaIsla(List<GameObject> hexaIsla, List<SpriteRenderer> hexaImage, int islandsCount, int hexaImageList)
    {
        for (int i = 0; i < islandsCount; i++)
        {
            int numeroRandom = Random.Range(0, hexaIsla.Count);
            Debug.Log("Numero Random: " + numeroRandom);
            view.RPC("ChangeIslandColor", RpcTarget.All, (int)numeroRandom, (float)255f, (float)165f, (float)0f, (int)hexaImageList);
            hexaIsla[numeroRandom].GetComponent<Animator>().SetBool("isTemblando", true);
            yield return new WaitForSeconds(10);
            hexaIsla[numeroRandom].GetComponent<Animator>().SetBool("isTemblando", false);
            hexaIsla[numeroRandom].GetComponent<Transform>().LeanMoveLocalY(elevationPreFall, 1);
            yield return new WaitForSeconds(1);
            view.RPC("ChangeIslandColor", RpcTarget.All, (int)numeroRandom, (float)255f, (float)0f, (float)0f, (int)hexaImageList);
            hexaIsla[numeroRandom].GetComponent<IslandFall>().isFalling = true;
            hexaIslandsFall.Add(hexaIsla[numeroRandom]);
            hexaIsla.RemoveAt(numeroRandom);
            hexaImage.RemoveAt(numeroRandom);
        }
        yield break;
    }

    // Cambia el color del trozo de isla en el minimapa para indicar que está por caerse.
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
