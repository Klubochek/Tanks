using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class TankNetworkRoomManager : NetworkRoomManager
{

    public List<TankNetworkRoomPlayer> tankRoomPlayers = new List<TankNetworkRoomPlayer>();
    public List<GameObject> CurrentPlayerTanks = new List<GameObject>();
    [SerializeField] private List<GameObject> _tankPrefabs;
    [SerializeField] private GameObject _content;
    [SerializeField] private TeamManager _teamManager;

    private MongoModule _mongoModule;

    #region ServerCallbacks
    public override void OnStartServer()
    {
        base.OnStartServer();
        _mongoModule = new MongoModule(true);
    }
    public override void OnStopServer()
    {
        base.OnStopServer();
        _mongoModule.DeleteServerData();
    }
    public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
    {
        var tankPlayer = tankRoomPlayers.Find(x => x.connectionToClient == conn);

        var tankStats = gamePlayer.GetComponent<TankStats>();
        tankStats.HP = tankStats.MAXHP;
        tankStats.Nickname = tankPlayer.PlayerName;
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
        if (_teamManager == null) _teamManager = FindObjectOfType<TeamManager>();

        var teamPlayersCount = _teamManager.CurrentTeams;
        var position = startPositions[team * 5 + teamPlayersCount[team]];
        return position;

    }

    public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
    {
        var team = roomPlayer.GetComponent<TankNetworkRoomPlayer>().Team;
        Debug.Log("Team:" + team);


        Transform startPos = ChooseStartPosition(team);
        playerPrefab = spawnPrefabs[team];
        GameObject TankPlayer = Instantiate(playerPrefab, startPos.position, startPos.rotation);



        CurrentPlayerTanks.Add(TankPlayer);
        _teamManager.AddTankToTeam(team);
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
    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        if (IsSceneActive(GameplayScene)) _teamManager.RemoveTankFromTeam(conn.identity.gameObject.GetComponent<TankStats>().Team);

        base.OnServerDisconnect(conn);
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
