using Mirror;
using System;
using TMPro;
using UnityEngine;

public class TankStats : NetworkBehaviour
{

    [SerializeField] public int ShellCount = 50;

    [SerializeField] private TankNetworkRoomPlayer tankPlayer;
    [SerializeField] private TankNetworkRoomManager Room;
    [SerializeField] private TextMeshPro tankTextNameAndHp;
    [SerializeField] private TankNetworkRoomManager tankNetworkRoomManager;
    [SerializeField] public HingeJoint joint;

    [SerializeField] public readonly int MAXHP = 10;


    [SyncVar(hook = nameof(HandleNicknameChanged))]
    public string nickname = "Loading...";
    [SyncVar(hook = nameof(HandleHPChanged))]
    public int hp = 0;
    [SyncVar]
    public int Team;


    public override void OnStartAuthority()
    {
        Room = FindObjectOfType<TankNetworkRoomManager>();
        tankPlayer = Room.tankRoomPlayers.Find(x => x.isLocalPlayer == true);
    }
    public void Damage()
    {
        CmdDamage();
    }
    [Command]
    public void CmdDamage()
    {
        Console.WriteLine("Damage");
        hp--;
    }
    [Command]
    public void CmdDeath()
    {
        if (Team == 0)
        {
            Room.CountOfDeathPlayer++;
            Room.lastBluePos--;
        }
        if (Team == 1)
        {
            Room.CountOfDeathPlayer++;
            Room.lastYellowPos--;
        }
        if (Team == 2)
        {
            Room.CountOfDeathPlayer++;
            Room.lastGreenPos--;
        }
        if (Team == 3)
        {
            Room.CountOfDeathPlayer++;
            Room.lastBrownPos--;
        }
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
