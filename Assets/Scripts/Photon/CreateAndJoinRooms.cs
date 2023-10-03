using Photon.Pun;
using System;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI joinInput;

    public void CreateRoom()
    {
        string newRoomCode = CreateRoomCode(6);
        // TODO: Validate Room does not exist
        PhotonNetwork.CreateRoom(newRoomCode);
    }

    private static Random random = new Random();
    public string CreateRoomCode(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
    public void JoinRoom()
    {
        // Adds in a random invisible letter at the end of the text???? I love me some zero width non printing characters !! :)
        PhotonNetwork.JoinRoom(joinInput.text.Remove(joinInput.text.Length - 1));
    }
    public void BackToTitleScreen()
    {
        PhotonNetwork.LeaveLobby();
    }

    public override void OnLeftLobby()
    {
        SceneManager.LoadScene("Title Screen");
    }
}
