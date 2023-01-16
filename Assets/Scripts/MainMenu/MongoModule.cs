using Realms;
using Realms.Sync;
using System;
using System.Linq;
using System.Net;
using MongoDB.Bson;

public class MongoModule
{
    public MongoModule(bool isServer = false)
    {
        StartMongoAuth(isServer);
    }

    private App _app;
    private string _myAppId = "tanksapp-aeebe";
    private Realm _realm;
    private User _user;
    private TankServer _tankServer;
    private string _NewServerName;

    public async void StartMongoAuth(bool isServer = false)
    {

        _app = App.Create(new AppConfiguration(_myAppId));
        _user = await _app.LogInAsync(Credentials.Anonymous(false));

        var config = new FlexibleSyncConfiguration(_user)
        {
            PopulateInitialSubscriptions = (realm) =>
            {
                var myItems = realm.All<TankServer>();
                realm.Subscriptions.Add(myItems);
            }
        };
       
        _realm = await Realm.GetInstanceAsync(config);


        if (isServer) SetNewServerData();

    }
    #region Client
    public IQueryable<TankServer> LoadServers()
    {
        var servers = _realm.All<TankServer>();
        return servers;
    }
    #endregion
    #region Server
    private void SetNewServerData()
    {
        string externalIpString = new WebClient().DownloadString("http://icanhazip.com").Replace("\\r\\n", "").Replace("\\n", "").Trim();
        var externalIp = IPAddress.Parse(externalIpString);
        

        Random random = new Random();

        _NewServerName = $"Server{random.Next(0, 10)}";
        _tankServer = new TankServer { IP = externalIp.ToString(), ServerName = _NewServerName,Id=new ObjectId(_user.Id) };
        _realm.Write(() => _realm.Add(_tankServer));
        Console.WriteLine("Current ip:" + _tankServer.IP);
        Console.WriteLine("Current ServerName:" + _tankServer.ServerName);
        Console.WriteLine("Current id" + _tankServer.Id);
    }
    public void DeleteServerData()
    {
        _realm.Write(() => _realm.Remove(_tankServer));
        Console.WriteLine("server was deleted");

    }
    #endregion
}
