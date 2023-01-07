using Firebase.Database;
using Mirror;
using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class TankNetworkRoomManager : NetworkRoomManager
{

    public List<TankNetworkRoomPlayer> tankRoomPlayers = new List<TankNetworkRoomPlayer>();
    public List<GameObject> CurrentPlayerTanks = new List<GameObject>();
    [SerializeField] private List<GameObject> tankPrefabs;
    [SerializeField] private GameObject content;
    public string ServerName;

    [SerializeField] private TeamManager teamManager;


    //[SyncVar(hook = nameof(HandleDeathCountChanged))]
    public int CountOfDeathPlayer = 0;

    public List<int> teamPlayersCount = new List<int>() { 0, 0, 0, 0 };

    public int lastBluePos = 0;
    public int lastYellowPos = 0;
    public int lastGreenPos = 0;
    public int lastBrownPos = 0;

    //private DatabaseReference dbr;


    #region ServerCallbacks

    public override void OnStartServer()
    {
        base.OnStartServer();

        //teamManager = FindObjectOfType<TeamManager>();
        //dbr = FirebaseDatabase.DefaultInstance.RootReference;


        string externalIpString = new WebClient().DownloadString("http://icanhazip.com").Replace("\\r\\n", "").Replace("\\n", "").Trim();
        var externalIp = IPAddress.Parse(externalIpString);
        Console.WriteLine("Current ip:" + externalIp);
        ServerName = $"Server{UnityEngine.Random.Range(0, 10)}";

        //dbr.Child("Servers").Child(ServerName).Child("NameOfServer").SetValueAsync(ServerName);
        //dbr.Child("Servers").Child(ServerName).Child("Ip").SetValueAsync(externalIp.ToString());


    }
    public override void OnStopServer()
    {
        base.OnStopServer();
        //dbr.Child("Servers").Child(ServerName).RemoveValueAsync();
    }

    public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
    {

        var tankPlayer = tankRoomPlayers.Find(x => x.connectionToClient == conn);

        if (tankPlayer == null) System.Console.WriteLine(tankPlayer);



        var tankStats = gamePlayer.GetComponent<TankStats>();

        tankStats.hp = tankStats.MAXHP;

        tankStats.nickname = tankPlayer.PlayerName;

        tankStats.Team = tankPlayer.Team;
        

        return true;
    }

    public override void OnRoomClientSceneChanged()
    {
        Debug.Log("SceneChanged");
        foreach (var tank in CurrentPlayerTanks)
        {
            tank.GetComponent<TankStats>().UpdateTankNameAndHp();
        }
    }
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {


        base.OnServerAddPlayer(conn);

        if (tankRoomPlayers.Count == 0)
            conn.identity.GetComponent<TankNetworkRoomPlayer>().IsLeader = true;


    }

    public Transform ChooseStartPosition(int team)
    {
        ;
        if (team == 0)
        {
            var position = startPositions[team * 5 + teamPlayersCount[team]];
            teamPlayersCount[team]++;
            return position;

        }
        if (team == 1)
        {
            var position = startPositions[team * 5 + teamPlayersCount[team]];
            teamPlayersCount[team]++;
            return position;
        }
        if (team == 2)
        {
            var position = startPositions[team * 5 + teamPlayersCount[team]];
            teamPlayersCount[team]++;
            return position;
        }
        if (team == 3)
        {
            var position = startPositions[team * 5 + teamPlayersCount[team]];
            teamPlayersCount[team]++;
            return position;
        }
        return null;
    }

    public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
    {
        var team = roomPlayer.GetComponent<TankNetworkRoomPlayer>().Team;
        Transform startPos = ChooseStartPosition(team);
        Debug.Log("Team:" + team);
        playerPrefab = spawnPrefabs[team];

        GameObject TankPlayer = Instantiate(playerPrefab, startPos.position, startPos.rotation);
        CurrentPlayerTanks.Add(TankPlayer);

        if (teamManager == null) teamManager = FindObjectOfType<TeamManager>();
        teamManager.AddTankToTeam(team);
        return TankPlayer;
    }
    public override void OnClientSceneChanged()
    {
        base.OnClientSceneChanged();
        foreach (var roomPlayer in tankRoomPlayers)
        {
            roomPlayer.SetupInGameUI();
        }
    }
    #endregion
    public void StartGame()
    {
        foreach (var player in tankRoomPlayers)
        {
            player.IsReady = true;
            player.readyToBegin = true;
        }
        OnRoomServerPlayersReady();
        
    }
    
}
