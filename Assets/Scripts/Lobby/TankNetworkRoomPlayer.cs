using Mirror;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TankNetworkRoomPlayer : NetworkRoomPlayer
{
    public Dictionary<int, Color32> TeamColor = new Dictionary<int, Color32>()
    {
        {0,Color.blue},
        {1,Color.yellow },
        {2,Color.green },
        {3,new Color32(102,51,0,255)}

    };




    [SyncVar(hook = nameof(HandleDisplayNameChanged))]
    public string PlayerName = "Loading...";
    [SyncVar(hook = nameof(HandleReadyStatusChanged))]
    public bool IsReady = false;

    public bool IsLeader = false;
    [SyncVar(hook = nameof(HandleTeamChanged))]
    public int Team = 0;



    [SerializeField] private PlayerData playerData;
    [SerializeField] private GameObject content;
    [SerializeField] private int maxTeamCount = 4;

    private TankNetworkRoomManager room;
    private TankNetworkRoomManager Room
    {
        get
        {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as TankNetworkRoomManager;
        }
    }
    #region Callbacks
    public override void OnStartAuthority()
    {
        CmdSetDisplayName(playerData.PlayerName);
    }
    public override void OnStartClient()
    {
        Room.tankRoomPlayers.Add(this);
        CmdSetTankPlayer();
        UpdateDisplay();
    }
    public override void OnStopClient()
    {
        Room.tankRoomPlayers.Remove(this);
        CmdRemoveTankPlayer();
        UpdateDisplay();
    }
    #endregion
    public void StartGame()
    {
        if (IsLeader)
            CmdStartGame();
    }

    public void Disconnect()
    {
        CmdDisconnect();
    }

    private void UpdateDisplay()
    {
        if (!isOwned)
        {
            foreach (var player in Room.tankRoomPlayers)
            {
                if (player.isOwned)
                {
                    player.UpdateDisplay();
                    break;
                }
            }

            return;
        }
        content = GameObject.FindGameObjectWithTag("Content");
        if (content != null)
        {
            for (int i = 0; i < Room.maxConnections; i++)
            {

                content.transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Empty";
                content.transform.GetChild(i).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = string.Empty;
                content.transform.GetChild(i).GetComponent<Image>().color = TeamColor[0];
            }

            for (int i = 0; i < Room.tankRoomPlayers.Count; i++)
            {
                Debug.Log("Updating Status");
                content.transform.GetChild(i).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Room.tankRoomPlayers[i].PlayerName;
                Debug.Log("Current status" + Room.tankRoomPlayers[i].IsReady);
                content.transform.GetChild(i).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = Room.tankRoomPlayers[i].IsReady ? "<color=green>Ready</color>" : "<color=red>Not Ready</color>";
                content.transform.GetChild(i).GetComponent<Image>().color = TeamColor[Room.tankRoomPlayers[i].Team];
            }
        }

    }
    public void SetupInGameUI()
    {
        if (SceneManager.GetActiveScene().name == "OnlineScene")
        {
            FindObjectOfType<InGameUI>().tankNetwork = this;
        }
        else
        {
            CmdBackToLobby();
        }
    }
    #region Commands
    [Command]
    private void CmdBackToLobby()
    {
        IsReady = false;
        FindObjectOfType<LobbyUI>().TankNetworkRoomPlayer = this;
        content = GameObject.FindGameObjectWithTag("Content");
        UpdateDisplay();
    }
    [Command]
    public void CmdSetTeam()
    {
        if (Team == maxTeamCount - 1) Team = 0;
        else Team++;
        UpdateDisplay();

    }
    [Command]
    private void CmdSetDisplayName(string playerName)
    {
        PlayerName = playerName;
    }
    [Command]
    public void CmdDisconnect()
    {
        Debug.Log("Disconnectiong");
        connectionToClient.Disconnect();
    }
    [Command]
    public void CmdStartGame()
    {
        if (Room.tankRoomPlayers[0].connectionToClient != connectionToClient) { return; }

        Room.StartGame();
    }
    [Command]
    public void CmdReadyUp()
    {
        IsReady = !IsReady;
        Debug.Log(IsReady);
    }
    //ТЕстовые команды
    [Command]
    private void CmdSetTankPlayer()
    {
        Room.tankRoomPlayers.Add(this);
    }
    [Command]
    private void CmdRemoveTankPlayer()
    {
        Room.tankRoomPlayers.Remove(this);
    }
    #endregion


    public void HandleReadyStatusChanged(bool oldValue, bool newValue) => UpdateDisplay();
    public void HandleDisplayNameChanged(string oldValue, string newValue) => UpdateDisplay();
    public void HandleTeamChanged(int oldValue, int newValue) => UpdateDisplay();
}
