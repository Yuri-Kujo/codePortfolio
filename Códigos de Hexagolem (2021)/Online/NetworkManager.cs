using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    byte maxPlayerPerRoom = 8;
    public bool conectadoAlServidor;

    public static NetworkManager network;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (network == null)
        {
            network = this;
        }

        else
        {
            Destroy(gameObject);
        }

        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    private void Update()
    {

    }

    public override void OnConnectedToMaster()
    {
        conectadoAlServidor = true;
        Debug.LogWarning("Conectado a servidor");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayerPerRoom });
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
    }

    public void DesconectarJugador()
    {
        StartCoroutine(DesconectarYCargar());
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Conectado()
    {
        if (PhotonNetwork.IsConnected)
        {
            // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
            Debug.LogWarning("Conectado a sala de espera");
            PhotonNetwork.JoinRandomRoom();
            PhotonNetwork.LoadLevel("Sala");
        }
    }

    IEnumerator DesconectarYCargar()
    {
        Time.timeScale = 1f;
        PhotonNetwork.LeaveRoom();
        while (PhotonNetwork.InRoom) yield return null;
        PhotonNetwork.LoadLevel("MainMenu");
        Debug.Log("loading main");
    }
}
