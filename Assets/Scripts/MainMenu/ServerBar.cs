using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ServerBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI serverText;
    [SerializeField] private TextMeshProUGUI ipAdresText;
    [SerializeField] private TankNetworkRoomManager tankNetworkRoomManager;

    private void Start()
    {
        tankNetworkRoomManager=FindObjectOfType<TankNetworkRoomManager>();
    }
    public void SetupServerParams(string serverName,string ip)
    {
        serverText.text = serverName;
        ipAdresText.text = ip;
    }
    public void OnConnectButtonClick()
    {
        tankNetworkRoomManager.networkAddress=ipAdresText.text;
        tankNetworkRoomManager.StartClient();
    }
}
