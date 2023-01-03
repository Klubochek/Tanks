using TMPro;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private GameObject inGameMemu;
    [SerializeField] private GameObject backToLobbyButton;
    [SerializeField] private TankNetworkRoomManager Room;
    [SerializeField] private TankNetworkRoomPlayer _tankNetwork;
    [SerializeField] private TextMeshProUGUI cdText;
    [SerializeField] private TextMeshProUGUI shellCountText;
    public TankNetworkRoomPlayer tankNetwork
    {
        get { return _tankNetwork; }
        set
        {
            _tankNetwork = value;
            UpDateUI();
        }
    }

    private void Start()
    {
        Room = FindObjectOfType<TankNetworkRoomManager>();
    }
    public void UpdateCDandShell(int count, bool hasCD)
    {
        shellCountText.text = "Shell Count:" + count.ToString();
        if (hasCD)
            cdText.text = $"<color=red> Reloading...</color>";
        else
            cdText.text = $"<color=green> Ready to fire</color>";
    }
    private void UpDateUI()
    {
        if (_tankNetwork.IsLeader)
        {
            backToLobbyButton.SetActive(true);
        }
    }
    public void OnDisconnectButtonClick()
    {
        Debug.Log("Disconneting...");
        foreach (var player in Room.tankRoomPlayers)
        {
            if (player.isLocalPlayer)
            {
                if (player.IsLeader)
                {
                    Room.StopHost();
                    break;
                }
                player.CmdDisconnect();
                break;
            }
        }

    }
    public void OnContinueButtonClick()
    {
        inGameMemu.SetActive(false);
    }
    public void OnMenuButtonClick()
    {
        inGameMemu.SetActive(true);
    }
    public void OnBackToLobbyButtonClick()
    {
        Room.ServerChangeScene(Room.RoomScene);
    }
}
