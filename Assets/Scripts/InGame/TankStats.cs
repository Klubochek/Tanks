using Mirror;
using TMPro;
using UnityEngine;

public class TankStats : NetworkBehaviour
{

    [SerializeField] public int Team;
    [SerializeField] public int ShellCount = 50;

    [SerializeField] private TankNetworkRoomPlayer tankPlayer;
    [SerializeField] private TankNetworkRoomManager Room;
    [SerializeField] private TextMeshPro tankTextNameAndHp;
    [SerializeField] private TankNetworkRoomManager tankNetworkRoomManager;
    [SerializeField] public HingeJoint joint;

    [SerializeField] public readonly int MAXHP = 3;


    [SyncVar(hook = nameof(HandleNicknameChanged))]
    public string nickname = "Loading...";
    [SyncVar(hook = nameof(HandleHPChanged))]
    public int hp = 0;



    public override void OnStartAuthority()
    {
        Room = FindObjectOfType<TankNetworkRoomManager>();
        tankPlayer = Room.tankRoomPlayers.Find(x => x.isLocalPlayer == true);
    }
    public void Damage()
    {
        CmdDamage();
    }
    [Command(requiresAuthority = false)]
    public void CmdDamage()
    {
        hp--;
    }
    public void UpdateTankNameAndHp()
    {
        Debug.Log("UpdateName");
        tankTextNameAndHp.text = nickname + "\tHP:" + hp;
    }

    public void DecreaseShellCount()
    {
        ShellCount--;
    }
    private void HandleNicknameChanged(string oldValue, string newValue) => UpdateTankNameAndHp();
    private void HandleHPChanged(int oldValue, int newValue) => UpdateTankNameAndHp();




}
