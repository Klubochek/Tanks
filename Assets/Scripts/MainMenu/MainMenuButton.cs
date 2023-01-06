using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Firebase.Database;
using System.Linq;

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

    private void Start()
    {
        Cursor.visible = true;
        currentUserName.text ="Current User:"+"\n"+playerData.PlayerName;
        dbr = FirebaseDatabase.DefaultInstance.RootReference;
    }
    public void OnJoinButtonClick()
    {
        StartCoroutine(Join());
    }
    public IEnumerator Join()
    {
        connectionMenu.SetActive(true);
        var servers = dbr.Child("Servers").GetValueAsync() ;
        yield return new WaitUntil(() => servers.IsCompleted);
        if (servers == null)
        {
            Debug.Log("NullServerData");
        }
        else if (servers.Result.Value == null)
        {
            Debug.Log("Empty db");
        }
        else
        {
            DataSnapshot ds = servers.Result;
            foreach (DataSnapshot dataSnapshot in ds.Children.Reverse())
            {
                GameObject serverBar = Instantiate(serverPrefab, content.transform);
                serversList.Add(serverBar);

                serverBar.GetComponent<ServerBar>().SetupServerParams(dataSnapshot.Child("NameOfServer").Value.ToString(), dataSnapshot.Child("Ip").Value.ToString());
            }
        }
    }
    public void OnConnectionWithOwnIpClick()
    {
        connector.Join(ip.text);
    }
    public void OnCloseConnectionMenuButtonClick()
    {
        foreach (var server in serversList)
        {
            Destroy(server);
        }
        serversList.Clear();
        connectionMenu.SetActive(false);
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
