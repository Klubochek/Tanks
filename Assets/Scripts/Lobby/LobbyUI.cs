using Mirror;
using UnityEngine;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private GameObject _startGameButton;
    private TankNetworkRoomPlayer _tankNetworkRoomPlayer;
    public TankNetworkRoomPlayer TankNetworkRoomPlayer
    {
        get { return _tankNetworkRoomPlayer; }
        set
        {
            _tankNetworkRoomPlayer = value;
            if (_tankNetworkRoomPlayer.IsLeader)
            {
                _startGameButton.SetActive(true);
            }
        }
    }

    private TankNetworkRoomManager _room;
    private TankNetworkRoomManager Room
    {
        get
        {
            if (_room != null) { return _room; }
            return _room = NetworkManager.singleton as TankNetworkRoomManager;
        }
    }
    private void Start()
    {
        Cursor.visible = true;
    }
    public void OnReadyButtonClick()
    {
        foreach (var player in Room.tankRoomPlayers)
        {
            if (player.isLocalPlayer)
            {
                Debug.Log(player.index);
                player.CmdReadyUp();
                player.CmdChangeReadyState(!player.readyToBegin);
                break;
            }
        }

    }
    public void OnStartButtonClick()
    {
        foreach (var player in Room.tankRoomPlayers)
        {
            if (player.IsLeader)
            {
                player.StartGame();
                break;
            }
        }
    }
    public void OnChangeTeamButtonClick()
    {
        Room.tankRoomPlayers.Find(x => x.isLocalPlayer == true).CmdSetTeam();
    }
    public void OnDisconnectButtonClick()
    {
        foreach (var player in Room.tankRoomPlayers)
        {
            if (player.isLocalPlayer)
            {
                if (player.IsLeader)
                {
                    Room.StopHost();
                    break;
                }
                else
                {
                    Debug.Log("Disconnect");
                    Room.StopClient();
                    break;
                }
            }
        }
    }
}
