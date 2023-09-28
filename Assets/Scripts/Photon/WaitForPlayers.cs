using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaitForPlayers : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI lobbyCodeText;
    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.CurrentRoom.Name);
        lobbyCodeText.text = PhotonNetwork.CurrentRoom.Name;
    }

}
