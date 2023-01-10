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
    [SerializeField] private Connector connector;
    [SerializeField] private GameObject connectionMenu;
    [SerializeField] private TMP_InputField ip;
    [SerializeField] private TextMeshProUGUI currentUserName;
    [SerializeField] private PlayerData playerData;
    

    
    [SerializeField] private ServersModule serversModule;
    private MongoModule mongoModule;
    
    private void Start()
    {
        mongoModule = new MongoModule();
        Cursor.visible = true;
        currentUserName.text = "Current User:" + "\n" + playerData.PlayerName;
    }
    
    public void OnJoinButtonClick()
    {
        StartCoroutine(Join());
    }
    public IEnumerator Join()
    {
        connectionMenu.SetActive(true);
        var allServers=mongoModule.LoadServers();
        yield return new WaitUntil(() => allServers != null);
        serversModule.CreateNewServerList(allServers);   
    }
    
    public void OnConnectionWithOwnIpClick()
    {
        connector.Join(ip.text);
    }
    public void OnCloseConnectionMenuButtonClick()
    {
        serversModule.DestroyServerBars();
        connectionMenu.SetActive(false);
    }
    public void OnRefreshButtonCLick()
    {
        serversModule.DestroyServerBars();
        StartCoroutine(Join());
    }

    public void OnHostButtonClick()
    {
        connector.Host();
    }
    public void OnExitButtonClick()
    {
        Application.Quit();
    }
}
