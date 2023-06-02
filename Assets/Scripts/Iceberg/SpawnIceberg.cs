using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SpawnIceberg : NetworkBehaviour
{
    public GameObject p_Iceberg;
    
    public bool gameStart = false;

    
    public void Start()
    {
        gameStart = true;
    }

    public void Update()
    {
        if (!isServer)
            return;

        if (gameStart == true)
        {
            CmdSpawn();
            gameStart = false;
        }  
    }

    [Command]
    public void CmdSpawn()
    {
        GameObject prefabGO = Instantiate(p_Iceberg);
        NetworkServer.Spawn(prefabGO, connectionToClient);
       
    }



}
