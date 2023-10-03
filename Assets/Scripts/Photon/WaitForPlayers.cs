using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaitForPlayers : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI lobbyCodeText;
    public TextMeshProUGUI playerCountText;
    public GameObject startGameButton;
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room: " + PhotonNetwork.CurrentRoom.Name);
        lobbyCodeText.text = PhotonNetwork.CurrentRoom.Name;
        playerCountText.text = PhotonNetwork.PlayerList.Length.ToString();
        AllowStartGame();
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        playerCountText.text = PhotonNetwork.PlayerList.Length.ToString();
        AllowStartGame();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join room due to: " + message);
    }

    void AllowStartGame()
    {
        if (PhotonNetwork.PlayerList.Length >= 2)
            startGameButton.SetActive(true);
    }
}
