using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ServersModule : MonoBehaviour
{
    [SerializeField] private List<GameObject> _serversList;
    [SerializeField] private GameObject _serverPrefab;
    [SerializeField] private GameObject _content;

    public void CreateNewServerList(IQueryable<TankServer> allServers)
    {

        if (allServers == null)
        {
            Debug.Log("Null data");
            return;
        }
        foreach (var server in allServers)
        {
            GameObject serverBar = Instantiate(_serverPrefab, _content.transform);
            _serversList.Add(serverBar);

            serverBar.GetComponent<ServerBar>().SetupServerParams(server.ServerName, server.IP);
        }
    }
    public void DestroyServerBars()
    {
        foreach (var server in _serversList)
        {
            Destroy(server);
        }
        _serversList.Clear();
    }
}
