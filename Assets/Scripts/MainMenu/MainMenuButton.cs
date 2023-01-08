using Firebase.Database;
using MongoDB.Bson;
using Realms;
using Realms.Sync;
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
    [SerializeField] private GameObject serverPrefab;
    [SerializeField] private GameObject content;
    private DatabaseReference dbr;

    [SerializeField] private List<GameObject> serversList;
    private App app;
    private string myAppId = "tanks-oyghl";
    private Realm realm;
    private User user;


    private void Start()
    {
        StartMongoAuth();


        Cursor.visible = true;
        currentUserName.text = "Current User:" + "\n" + playerData.PlayerName;
        dbr = FirebaseDatabase.DefaultInstance.RootReference;
    }
    public async void StartMongoAuth()
    {
        app = App.Create(new AppConfiguration(myAppId));
        user = await app.LogInAsync(Credentials.Anonymous());

        var config = new PartitionSyncConfiguration(user.Id,user);

        // The process will complete when all the user's items have been downloaded.
        realm = await Realm.GetInstanceAsync(config);
    }


    public void OnJoinButtonClick()
    {
        StartCoroutine(Join());
    }
    public IEnumerator Join()
    {
        var tesetserver = new ServersCollection { IP = "ipaddress", ServerName = "serever2" };

        realm.Write(() => realm.Add(tesetserver));

        connectionMenu.SetActive(true);
        var allServers = realm.All<ServersCollection>();
        //Debug.Log(allServers[0].IP);
        
        
       
        yield return new WaitForSeconds(3);
        if (allServers == null)
        {
            Debug.Log("Null data");
            yield break;
        }
        foreach (var server in allServers)
        {
            GameObject serverBar = Instantiate(serverPrefab, content.transform);
            serversList.Add(serverBar);

            serverBar.GetComponent<ServerBar>().SetupServerParams(server.ServerName, server.IP);
        }

        //var servers = dbr.Child("Servers").GetValueAsync() ;
        //yield return new WaitUntil(() => servers.IsCompleted);
        //if (servers == null)
        //{
        //    Debug.Log("NullServerData");
        //}
        //else if (servers.Result.Value == null)
        //{
        //    Debug.Log("Empty db");
        //}
        //else
        //{
        //    DataSnapshot ds = servers.Result;
        //    foreach (DataSnapshot dataSnapshot in ds.Children.Reverse())
        //    {
        //        GameObject serverBar = Instantiate(serverPrefab, content.transform);
        //        serversList.Add(serverBar);

        //        serverBar.GetComponent<ServerBar>().SetupServerParams(dataSnapshot.Child("NameOfServer").Value.ToString(), dataSnapshot.Child("Ip").Value.ToString());
        //    }
        //}
    }
    public void OnConnectionWithOwnIpClick()
    {
        connector.Join(ip.text);
    }
    public void OnCloseConnectionMenuButtonClick()
    {
        DestroyServerBars();
        connectionMenu.SetActive(false);
    }
    public void OnRefreshButtonCLick()
    {
        DestroyServerBars();
        StartCoroutine(Join());

    }
    public void DestroyServerBars()
    {
        foreach (var server in serversList)
        {
            Destroy(server);
        }
        serversList.Clear();
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
