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
    private string userApiKey = "G2sZdejhcpl7SOvOeMztZFaQ7vsJYVX2GYZVm4zcVXEdIPtZHGaLrua39HniYJZn";

    //For server data
    private TankServer tankServer;
    private string NewServerName;

    public async void StartMongoAuth(bool isServer = false)
    {

        app = App.Create(new AppConfiguration(myAppId));
        user = await app.LogInAsync(Credentials.ApiKey(userApiKey));

        var config = new PartitionSyncConfiguration(user.Id, user);

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
