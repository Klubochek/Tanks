using TMPro;
using UnityEngine;

public class ServerBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _serverText;
    [SerializeField] private TextMeshProUGUI _ipAdresText;
    [SerializeField] private Connector _connector;

    private void Start()
    {
        _connector = FindObjectOfType<Connector>();
    }
    public void SetupServerParams(string serverName, string ip)
    {
        _serverText.text = serverName;
        _ipAdresText.text = ip;
    }
    public void OnConnectButtonClick()
    {
        _connector.Join(_ipAdresText.text);
    }
}
