using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

//------------------------------------------------------------------
// Se encarga de:
// - Settear inicialmente a la cantidad de jugadores en partida.
// - Ver cuantos hay en juego y cuantos han perdido.
// - Habilitar el modo espectador al perder.
// - Pausar el juego cuando ya hayan caido todos y nombrar al ganador.
//------------------------------------------------------------------

public class GameManager : MonoBehaviourPunCallbacks
{ 
    public List<GameObject> playersInGame;
    public List<GameObject> playersLost;
    private float collectPNumber = 2.5f;
    public int countPlayers;
    public int seterPlayerLostPlace;
    public GameObject specterCamera;
    public bool freezeGame;

    PhotonView view;
    private void Awake()
    {
        view = GetComponent<PhotonView>();
        if (!PhotonNetwork.InRoom)
        {
            PhotonNetwork.LoadLevel(1);
        }
    }
    public void Start()
    {
        StartCoroutine(CollectPlayers());
        countPlayers = PhotonNetwork.PlayerList.Length;
    }

    public void Update()
    {
        if(playersInGame.Count == 1)
        {
            playersInGame[0].GetComponent<ThirdControllerV2>().isWin = true;
            playersInGame[0].transform.GetChild(8).gameObject.SetActive(false);
            playersInGame[0].transform.GetChild(6).GetChild(0).gameObject.SetActive(false);

            if (freezeGame) return;
            PauseMatch();
        }
        playersInGame.RemoveAll(GameObject => GameObject == null);
    }

    public void PauseMatch()
    {
        Time.timeScale = 0f;
        freezeGame = true;
    }

    IEnumerator CollectPlayers()
    {
        yield return new WaitForSeconds(collectPNumber);
        playersInGame.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        specterCamera.SetActive(true);
        yield return null;
    }

    public void SetPlaceInLost()
    {
        seterPlayerLostPlace = countPlayers - (playersLost.Count - 1);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        
    }
}
