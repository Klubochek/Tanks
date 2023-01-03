using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class MainMenuButton : MonoBehaviour
{
    [SerializeField] private Connector connector;
    [SerializeField] private GameObject connectionMenu;
    [SerializeField] private TMP_InputField ip;
    [SerializeField] private TextMeshProUGUI currentUserName;
    [SerializeField] private PlayerData playerData;

    private void Start()
    {
        Cursor.visible = true;
        currentUserName.text ="Current User:"+"\n"+playerData.PlayerName;
    }
    public void OnJoinButtonClick()
    {
        connectionMenu.SetActive(true);
    }
    public void OnConnectionButtonClick()
    {
        connector.Join(ip.text);
    }
    public void OnCloseConnectionMenuButtonClick()
    {
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
