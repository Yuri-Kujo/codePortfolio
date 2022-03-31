using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class PlayerListItem : MonoBehaviour
{
    [SerializeField] TMP_Text playerName;
    [SerializeField] Image playerImage;
    Player player;

    public void SetUp(Player _player)
    {
        player = _player;

        playerName.text = _player.NickName;
    }
}
