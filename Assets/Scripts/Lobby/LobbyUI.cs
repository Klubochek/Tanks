using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyUI : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = true;
    }

    [SerializeField] private GameObject startGameButton;
    private TankNetworkRoomPlayer tankNetworkRoomPlayer;
    public TankNetworkRoomPlayer TankNetworkRoomPlayer
    {
        get { return tankNetworkRoomPlayer; }
        set
        {
            tankNetworkRoomPlayer = value;
            if (tankNetworkRoomPlayer.IsLeader)
            {
                startGameButton.SetActive(true);
                ;
            }
        }
    }

    private TankNetworkRoomManager room;
    private TankNetworkRoomManager Room
    {
        get
        {
            if (room != null) { return room; }
            return room = NetworkManager.singleton as TankNetworkRoomManager;
        }
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
                    player.CmdDisconnect();
                    break;
                }
            }
        }
    }
}
