using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkObjectDestroyer : NetworkBehaviour
{
    public static NetworkObjectDestroyer Instance;
    private void Awake()
    {
        // skip if not the local player
        if (!isLocalPlayer) return;

        // set the static instance
       Instance = this;
    }

    [Client]
    public void TellServerToDestroyObject(GameObject obj)
    {
        CmdDestroyObject(obj);
    }

    // Executed only on the server
    [Command]
    private void CmdDestroyObject(GameObject obj, NetworkConnectionToClient sender = null)
    {
        Debug.Log("Null? " + (obj == null));
        if (!obj) return;
        NetworkServer.Destroy(obj);
    }
}
