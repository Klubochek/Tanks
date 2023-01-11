using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class TankNetworkRoomManager : NetworkRoomManager
{

    public List<TankNetworkRoomPlayer> tankRoomPlayers = new List<TankNetworkRoomPlayer>();
    public List<GameObject> CurrentPlayerTanks = new List<GameObject>();
    [SerializeField] private List<GameObject> tankPrefabs;
    [SerializeField] private GameObject content;


    [SerializeField] private TeamManager teamManager;




    private MongoModule mongoModule;

    #region ServerCallbacks

    public override void OnStartServer()
    {
        base.OnStartServer();
        mongoModule = new MongoModule(true);
    }
    public override void OnStopServer()
    {
        base.OnStopServer();
        mongoModule.DeleteServerData();
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
        var teamPlayersCount = teamManager.CurrentTeams;
        if (team == 0)
        {
            var position = startPositions[team * 5 + teamPlayersCount[team]];
            return position;
        }
        if (team == 1)
        {
            var position = startPositions[team * 5 + teamPlayersCount[team]];
            return position;
        }
        if (team == 2)
        {
            var position = startPositions[team * 5 + teamPlayersCount[team]];
            
            return position;
        }
        if (team == 3)
        {
            var position = startPositions[team * 5 + teamPlayersCount[team]];
            return position;
        }
        return null;
    }

    public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
    {
        if (teamManager == null) teamManager = FindObjectOfType<TeamManager>();
        var team = roomPlayer.GetComponent<TankNetworkRoomPlayer>().Team;
        Transform startPos = ChooseStartPosition(team);
        Debug.Log("Team:" + team);
        playerPrefab = spawnPrefabs[team];

        GameObject TankPlayer = Instantiate(playerPrefab, startPos.position, startPos.rotation);
        CurrentPlayerTanks.Add(TankPlayer);

        
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
