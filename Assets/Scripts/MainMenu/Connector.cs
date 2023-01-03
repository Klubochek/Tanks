using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Connector : MonoBehaviour
{
    public NetworkRoomManager networkRoomManager;

    public void Join(string IP)
    {
        if (IP != string.Empty) 
        {
            networkRoomManager.networkAddress = IP;
            networkRoomManager.StartClient(); 
        }
            
        
        else {
            networkRoomManager.networkAddress = "localhost";
            networkRoomManager.StartClient();
        }

    }
    public void Host()
    {
        networkRoomManager.StartHost();
    }
}
