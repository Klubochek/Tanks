using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class TankNetworkRoomManager : NetworkRoomManager
{

    public List<TankNetworkRoomPlayer> tankRoomPlayers = new List<TankNetworkRoomPlayer>();
    public List<GameObject> CurrentPlayerTanks = new List<GameObject>();
    [SerializeField] private List<GameObject> tankPrefabs;
    [SerializeField] private GameObject content;

    //public override void OnStartServer()
    //{
    //    foreach (var tank in tankRoomPlayers)
    //    {
    //        spawnPrefabs.Add(tank.gameObject);
    //    }

    //}
    //public override void OnStartClient()
    //{
    //    //if ( SceneManager.GetActiveScene().name==onlineScene )
    //    foreach (var tank in tankPrefabs)
    //    {
    //        NetworkClient.RegisterPrefab(tank);
    //    }
    //}
    public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
    {
        var tankPlayer = tankRoomPlayers.Find(x => x.connectionToClient == conn);
        gamePlayer.GetComponent<TankStats>().hp = 3;
        gamePlayer.GetComponent<TankStats>().nickname=tankPlayer.PlayerName;
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
    public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
    {
        Transform startPos = GetStartPosition();
        Debug.Log("Team:" + roomPlayer.GetComponent<TankNetworkRoomPlayer>().Team);
        playerPrefab = spawnPrefabs[roomPlayer.GetComponent<TankNetworkRoomPlayer>().Team];

        GameObject TankPlayer = Instantiate(playerPrefab, startPos.position, startPos.rotation);
        CurrentPlayerTanks.Add(TankPlayer);
        return TankPlayer;
    }
    public void StartGame()
    {
        foreach (var player in tankRoomPlayers)
        {
            player.IsReady = true;
            player.readyToBegin = true;
        }
        OnRoomServerPlayersReady();
    }
    public override void OnClientSceneChanged()
    {
        base.OnClientSceneChanged();
        foreach (var roomPlayer in tankRoomPlayers)
        {
            roomPlayer.SetupInGameUI();
        }
    }
}
