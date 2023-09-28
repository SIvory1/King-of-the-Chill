using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject loadingMenu;
    [SerializeField] GameObject lobbyMenu;
    public void StartGame()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        lobbyMenu.SetActive(true);
        loadingMenu.SetActive(false);
    }
}
