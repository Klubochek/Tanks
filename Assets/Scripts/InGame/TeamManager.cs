using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class TeamManager : NetworkBehaviour
{
    public List<int> CurrentTeams = new List<int> { 0, 0, 0, 0 };
    [SerializeField] private InGameUI _inGameUI;

    public void AddTankToTeam(int team)
    {
        CurrentTeams[team]++;
        Console.WriteLine($"New tank added to team {team}. Current count of tank is {CurrentTeams[team]}");
    }
    public void RemoveTankFromTeam(int team)
    {
        CurrentTeams[team]--;
        Console.WriteLine($"Tank desroyed. Current tankcount in team {team} is {CurrentTeams[team]}");
        CheckCurrentTeamState();
    }

    private void CheckCurrentTeamState()
    {
        int emptyTeam = 0;
        for (int i = 0; i < CurrentTeams.Count; i++)
        {
            if (CurrentTeams[i] == 0)
            {
                emptyTeam++;
            }
        }
        Console.WriteLine("Now empty team is " + emptyTeam);
        if (emptyTeam == CurrentTeams.Count-1)
        {
            RpcShowWinner(CurrentTeams.FindIndex(x=>x>0));
            StartCoroutine(StopServer());
            
        }
    }
    [ClientRpc]
    private void RpcShowWinner(int teamWinner)
    {
        _inGameUI.ShowWinner(teamWinner);

    }
    private IEnumerator StopServer()
    {
        yield return new WaitForSeconds(5);
        var roomManager=FindObjectOfType<TankNetworkRoomManager>();
        roomManager.ServerChangeScene(roomManager.RoomScene);
    }
}
