using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ServersModule : MonoBehaviour
{
    [SerializeField] private List<GameObject> serversList;
    [SerializeField] private GameObject serverPrefab;
    [SerializeField] private GameObject content;

    public void CreateNewServerList(IQueryable<TankServer> allServers)
    {

        if (allServers == null)
        {
            Debug.Log("Null data");
            return;
        }
        foreach (var server in allServers)
        {
            GameObject serverBar = Instantiate(serverPrefab, content.transform);
            serversList.Add(serverBar);

            serverBar.GetComponent<ServerBar>().SetupServerParams(server.ServerName, server.IP);
        }
    }
    public void DestroyServerBars()
    {
        foreach (var server in serversList)
        {
            Destroy(server);
        }
        serversList.Clear();
    }
}
