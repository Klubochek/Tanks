using Mirror;
using System;
using TMPro;
using UnityEngine;

public class TankStats : NetworkBehaviour
{
    [SerializeField] private TeamManager _teamManager;
    [SerializeField] private TankNetworkRoomPlayer _tankPlayer;
    [SerializeField] private TankNetworkRoomManager _room;
    [SerializeField] private TextMeshPro _tankTextNameAndHp;
    [SerializeField] public HingeJoint _joint;

    [SerializeField] public int ShellCount = 50;
    [SerializeField] public readonly int MAXHP = 10;
    public bool IsDead = false;

    [SyncVar(hook = nameof(HandleNicknameChanged))]
    public string Nickname = "Loading...";
    [SyncVar(hook = nameof(HandleHPChanged))]
    public int HP = 0;
    [SyncVar]
    public int Team;



    public override void OnStartAuthority()
    {
        _room = FindObjectOfType<TankNetworkRoomManager>();
        _tankPlayer = _room.tankRoomPlayers.Find(x => x.isLocalPlayer == true);
    }

    public void Damage()
    {
        CmdDamage();
    }
    [Command]
    public void CmdAddPlayrToTeam()
    {
        _teamManager.AddTankToTeam(Team);
    }
    [Command]
    public void CmdDamage()
    {
        Console.WriteLine("Damage");
        HP--;
    }
    public void Death()
    {
        CmdDeath();
    }
    [Command]
    public void CmdDeath()
    {
        _teamManager = FindObjectOfType<TeamManager>();
        _teamManager.RemoveTankFromTeam(Team);

        RpcDeath();
    }

    [ClientRpc]
    private void RpcDeath()
    {
        IsDead = true;
    }

    public void UpdateTankNameAndHp()
    {
        Debug.Log("UpdateName");
        _tankTextNameAndHp.text = Nickname + "\tHP:" + HP;
    }

    public void DecreaseShellCount()
    {
        ShellCount--;
    }
    private void HandleNicknameChanged(string oldValue, string newValue) => UpdateTankNameAndHp();
    private void HandleHPChanged(int oldValue, int newValue) => UpdateTankNameAndHp();




}
