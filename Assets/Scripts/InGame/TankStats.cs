using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using System.Runtime.CompilerServices;
using System;

public class TankStats : NetworkBehaviour
{

    [SerializeField] public int Team { get; private set; }
    [SerializeField] public int ShellCount = 50;

    [SerializeField] private TankNetworkRoomPlayer tankPlayer;
    [SerializeField] private TankNetworkRoomManager Room;
    [SerializeField] private TextMeshPro tankTextNameAndHp;
    [SerializeField] private TankNetworkRoomManager tankNetworkRoomManager;
    [SerializeField] const int MAXHP = 3;


    [SyncVar(hook = nameof(HandleNicknameChanged))]
    public string nickname = "Loading...";



    [SyncVar(hook = nameof(HandleHPChanged))]
    public int hp = 0;


    public override void OnStartAuthority()
    {
        Room = FindObjectOfType<TankNetworkRoomManager>();
        tankPlayer = Room.tankRoomPlayers.Find(x => x.isLocalPlayer == true);
        //CmdSetNicknameAndHp();
    }
    [Command]
    public void CmdSetNicknameAndHp(string name)
    {
        nickname = name;
        hp = MAXHP;
    }
    [Command]
    public void CmdDamage() 
    {
        hp--;
    }
    public void UpdateTankNameAndHp()
    {
        Debug.Log("UpdateName");
        tankTextNameAndHp.text = nickname + "\tHP:" + hp;
        //CmdUpdateTankNameAndHp();
        //tankTextNameAndHp.text = nickname + "\tHP:" + hp;
    }

    [Command]
    public void CmdUpdateTankNameAndHp()
    {
        Debug.Log("CmdUpdateName"); 
        tankTextNameAndHp.text = nickname + "\tHP:" + hp;
        //RpcUpdateTankNameAndHp();
    }
    [ClientRpc]
    private void RpcUpdateTankNameAndHp()
    {
        Debug.Log("RpcUpdateName");
        tankTextNameAndHp.text = nickname + "\tHP:" + hp;
    }
    public void DecreaseShellCount()
    {
        ShellCount--;
    }
    private void HandleNicknameChanged(string oldValue, string newValue) => UpdateTankNameAndHp();
    private void HandleHPChanged(int oldValue, int newValue) => UpdateTankNameAndHp();




}
