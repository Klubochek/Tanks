using Realms;
using Realms.Sync;
using System;
using System.Linq;
using System.Net;

public class MongoModule
{
    public MongoModule(bool isServer = false)
    {
        StartMongoAuth(isServer);
    }

    private App app;
    private string myAppId = "tanksapp-aeebe";
    private Realm realm;
    private User user;
    private TankServer tankServer;
    private string NewServerName;

    public async void StartMongoAuth(bool isServer = false)
    {

        app = App.Create(new AppConfiguration(myAppId));
        user = await app.LogInAsync(Credentials.Anonymous(false));

        var config = new FlexibleSyncConfiguration(user)
        {
            PopulateInitialSubscriptions = (realm) =>
            {
                var myItems = realm.All<TankServer>();
                realm.Subscriptions.Add(myItems);
            }
        };
       
        realm = await Realm.GetInstanceAsync(config);


        if (isServer) SetNewServerData();

    }
    #region Client
    public IQueryable<TankServer> LoadServers()
    {
        var servers = realm.All<TankServer>();
        return servers;
    }
    #endregion
    #region Server
    private void SetNewServerData()
    {
        string externalIpString = new WebClient().DownloadString("http://icanhazip.com").Replace("\\r\\n", "").Replace("\\n", "").Trim();
        var externalIp = IPAddress.Parse(externalIpString);
        Console.WriteLine("Current ip:" + externalIp);

        Random random = new Random();

        NewServerName = $"Server{random.Next(0, 10)}";
        tankServer = new TankServer { IP = externalIp.ToString(), ServerName = NewServerName };
        realm.Write(() => realm.Add(tankServer));
    }
    public void DeleteServerData()
    {
        realm.Write(() => realm.Remove(tankServer));
    }
    #endregion
}
