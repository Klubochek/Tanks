using Mirror;
using Realms;
using Realms.Sync;
using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class TankNetworkRoomManager : NetworkRoomManager
{

    public List<TankNetworkRoomPlayer> tankRoomPlayers = new List<TankNetworkRoomPlayer>();
    public List<GameObject> CurrentPlayerTanks = new List<GameObject>();
    [SerializeField] private List<GameObject> tankPrefabs;
    [SerializeField] private GameObject content;
    

    [SerializeField] private TeamManager teamManager;

    public int CountOfDeathPlayer = 0;

    public List<int> teamPlayersCount = new List<int>() { 0, 0, 0, 0 };


    private App app;
    private string myAppId = "tanksapp-aeebe";
    private string serverApiKey = "3ajKNzxo4JTjscK850iegKyaVJKj8YhjFfGUVRCwUCf4MW4XP7hqQmO2qc0cuzxZ";
    private Realm realm;
    private User user;
    private TankServer tankServer;
    public string NewServerName;


    #region ServerCallbacks

    public override void OnStartServer()
    {
        base.OnStartServer();
        StartMongoAuth();



    }
    public void SetNewServerData()
    {
        string externalIpString = new WebClient().DownloadString("http://icanhazip.com").Replace("\\r\\n", "").Replace("\\n", "").Trim();
        var externalIp = IPAddress.Parse(externalIpString);
        Console.WriteLine("Current ip:" + externalIp);
        
        NewServerName = $"Server{UnityEngine.Random.Range(0, 10)}";
        tankServer = new TankServer { IP = externalIp.ToString(), ServerName = NewServerName };
        realm.Write(() => realm.Add(tankServer));
    }
    public async void StartMongoAuth()
    {
        app = App.Create(new AppConfiguration(myAppId));
        user = await app.LogInAsync(Credentials.ApiKey(serverApiKey));
        var config = new PartitionSyncConfiguration(user.Id, user);
        realm = await Realm.GetInstanceAsync(config);

        SetNewServerData();
    }
    public override void OnStopServer()
    {
        base.OnStopServer();
        realm.Write(() => realm.Remove(tankServer));
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
