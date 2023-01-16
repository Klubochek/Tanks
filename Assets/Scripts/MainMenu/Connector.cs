using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Connector : MonoBehaviour
{
    public NetworkRoomManager NetworkRoomManager;

    public void Join(string IP)
    {
        if (IP != string.Empty) 
        {
            NetworkRoomManager.networkAddress = IP;
            NetworkRoomManager.StartClient(); 
        }
            
        
        else {
            NetworkRoomManager.networkAddress = "localhost";
            NetworkRoomManager.StartClient();
        }

    }
    public void Host()
    {
        NetworkRoomManager.StartHost();
    }
}
