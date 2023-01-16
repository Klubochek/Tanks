using TMPro;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private GameObject _inGameMemu;
    [SerializeField] private GameObject _backToLobbyButton;
    [SerializeField] private TankNetworkRoomManager _room;
    [SerializeField] private TankNetworkRoomPlayer _tankNetwork;
    [SerializeField] private TextMeshProUGUI _cdText;
    [SerializeField] private TextMeshProUGUI _shellCountText;
    [SerializeField] private TextMeshProUGUI _endGameText;
    public TankNetworkRoomPlayer TankNetwork
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
        _room = FindObjectOfType<TankNetworkRoomManager>();
    }
    public void UpdateCDandShell(int count, bool hasCD)
    {
        _shellCountText.text = "Shell Count:" + count.ToString();
        if (hasCD)
            _cdText.text = $"<color=red> Reloading...</color>";
        else
            _cdText.text = $"<color=green> Ready to fire</color>";
    }
    private void UpDateUI()
    {
        if (_tankNetwork.IsLeader)
        {
            _backToLobbyButton.SetActive(true);
        }
    }
    public void OnDisconnectButtonClick()
    {
        Debug.Log("Disconneting...");
        foreach (var player in _room.tankRoomPlayers)
        {
            if (player.isOwned)
            {
                if (player.IsLeader)
                {
                    _room.StopHost();
                    break;
                }
                player.CmdDisconnect();
                break;
            }
        }

    }
    public void OnContinueButtonClick()
    {
        _inGameMemu.SetActive(false);
    }
    public void OnMenuButtonClick()
    {
        _inGameMemu.SetActive(true);
    }
    public void OnBackToLobbyButtonClick()
    {
        _room.ServerChangeScene(_room.RoomScene);
    }
    public void ShowWinner(int team)
    {
        if (team == 0)
            _endGameText.text = "Победила голубая команда";
        if (team == 1)
            _endGameText.text = "Победила желтая команда";
        if (team == 2)
            _endGameText.text = "Победила зеленая команда";
        if (team == 3)
            _endGameText.text = "Победила коричневая команда";
    }
}
