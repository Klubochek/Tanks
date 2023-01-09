using Realms;
using Realms.Sync;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

    [SerializeField] private List<GameObject> serversList;


    
    private App app;
    private string myAppId = "tanksapp-aeebe";
    private Realm realm;
    private User user;
    private string userApiKey = "3ajKNzxo4JTjscK850iegKyaVJKj8YhjFfGUVRCwUCf4MW4XP7hqQmO2qc0cuzxZ";
    private IQueryable<TankServer> allServers;

    private void Start()
    {
        StartMongoAuth();


        Cursor.visible = true;
        currentUserName.text = "Current User:" + "\n" + playerData.PlayerName;
    }
    public async void StartMongoAuth()
    {
        app = App.Create(new AppConfiguration(myAppId));

        user = await app.LogInAsync(Credentials.ApiKey(userApiKey));

        var config = new PartitionSyncConfiguration(user.Id, user);

        realm = await Realm.GetInstanceAsync(config);
    }


    public void OnJoinButtonClick()
    {
        StartCoroutine(Join());
    }
    public IEnumerator Join()
    {
        connectionMenu.SetActive(true);
        
           allServers=realm.All<TankServer>(); 
        



        yield return new WaitUntil(()=>allServers!=null);
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
