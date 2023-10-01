using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaitForPlayers : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI lobbyCodeText;
    public TextMeshProUGUI playerCountText;
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room: " + PhotonNetwork.CurrentRoom.Name);
        lobbyCodeText.text = PhotonNetwork.CurrentRoom.Name;
        playerCountText.text = PhotonNetwork.PlayerList.Length.ToString();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join room due to: " + message);
    }

}
