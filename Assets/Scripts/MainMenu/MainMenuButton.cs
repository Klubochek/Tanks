using Realms;
using Realms.Sync;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class MainMenuButton : MonoBehaviour
{
    [SerializeField] private Connector _connector;
    [SerializeField] private GameObject _connectionMenu;
    [SerializeField] private TMP_InputField _ip;
    [SerializeField] private TextMeshProUGUI _currentUserName;
    [SerializeField] private PlayerData _playerData;
    

    
    [SerializeField] private ServersModule _serversModule;
    private MongoModule _mongoModule;
    
    private void Start()
    {
        _mongoModule = new MongoModule();
        Cursor.visible = true;
        _currentUserName.text = "Current User:" + "\n" + _playerData.PlayerName;
    }
    
    public void OnJoinButtonClick()
    {
        StartCoroutine(Join());
    }
    public IEnumerator Join()
    {
        _connectionMenu.SetActive(true);
        var allServers=_mongoModule.LoadServers();
        yield return new WaitUntil(() => allServers != null);
        _serversModule.CreateNewServerList(allServers);   
    }
    
    public void OnConnectionWithOwnIpClick()
    {
        _connector.Join(_ip.text);
    }
    public void OnCloseConnectionMenuButtonClick()
    {
        _serversModule.DestroyServerBars();
        _connectionMenu.SetActive(false);
    }
    public void OnRefreshButtonCLick()
    {
        _serversModule.DestroyServerBars();
        StartCoroutine(Join());
    }

    public void OnHostButtonClick()
    {
        _connector.Host();
    }
    public void OnExitButtonClick()
    {
        Application.Quit();
    }
}
