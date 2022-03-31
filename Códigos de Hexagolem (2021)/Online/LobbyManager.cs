using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public Transform listaDeJugadores;
    public GameObject playerListItemPrefab;
    public GameObject startButton;
    public int playersInRoom;
    public bool canStart;
    public TextMeshProUGUI roomState;
    public TextMeshProUGUI countDown;
    private float timerCountDown = 5;
    private PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
        listaDeJugadores = GameObject.FindGameObjectWithTag("ListaDeJugadores").GetComponent<Transform>();
        roomState.text = "Room State: Open";
    }

    private void Update()
    {
        countDown.text = timerCountDown.ToString("f0");
        if (canStart)
        {
            timerCountDown -= Time.deltaTime;
        }
    }
    public override void OnJoinedRoom()
    {
        Player[] players = PhotonNetwork.PlayerList;

        playersInRoom = players.Count();

        foreach (Transform child in listaDeJugadores)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(playerListItemPrefab, listaDeJugadores).GetComponent<PlayerListItem>().SetUp(players[i]);
        }

        if (playersInRoom >= 2)
        {
            if (canStart) return;
            if(PhotonNetwork.IsMasterClient)
            {
                startButton.SetActive(true);
            }
            canStart = true;
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Player[] players = PhotonNetwork.PlayerList;

        playersInRoom = players.Count();

        foreach (Transform child in listaDeJugadores)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(playerListItemPrefab, listaDeJugadores).GetComponent<PlayerListItem>().SetUp(players[i]);
        }

        if (playersInRoom >= 2)
        {
            if (canStart) return;
            if (PhotonNetwork.IsMasterClient)
            {
                startButton.SetActive(true);
            }
            canStart = true;
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
    }

    public void CoroutineStarter()
    {
        startButton.SetActive(false);
        view.RPC("StartGameRPC", RpcTarget.Others);
        if(PhotonNetwork.IsMasterClient)
        {
            StartCoroutine("StartGame");
        }
    }

    [PunRPC]
    IEnumerator StartGameRPC()
    {
        countDown.gameObject.SetActive(true);
        roomState.text = "Room State: starting game";
        timerCountDown = 5;
        yield return new WaitForSeconds(5);
        yield break;
    }
    IEnumerator StartGame()
    {
        countDown.gameObject.SetActive(true);
        roomState.text = "Room State: starting game";
        timerCountDown = 5;
        yield return new WaitForSeconds(5);
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.LoadLevel("GameTest");
        yield break;
    }
}
