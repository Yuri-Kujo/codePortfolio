using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerNameManager : MonoBehaviour
{
    [SerializeField] TMP_InputField nombreUsuarioInput;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey("Nombre de usuario"))
        {
            nombreUsuarioInput.text = PlayerPrefs.GetString("Nombre de usuario");

            PhotonNetwork.NickName = PlayerPrefs.GetString("Nombre de usuario");
        }

        else
        {
            nombreUsuarioInput.text = "Rapi " + Random.Range(0, 10000).ToString("0000");
            EnNombreUsuarioInputValueChange();
        }
    }

    public void EnNombreUsuarioInputValueChange()
    {
        PhotonNetwork.NickName = nombreUsuarioInput.text;
        PlayerPrefs.SetString("Nombre de usuario", nombreUsuarioInput.text);
    }
}
