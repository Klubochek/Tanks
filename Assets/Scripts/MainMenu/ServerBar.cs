using TMPro;
using UnityEngine;

public class ServerBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI serverText;
    [SerializeField] private TextMeshProUGUI ipAdresText;
    [SerializeField] private Connector connector;

    private void Start()
    {
        connector = FindObjectOfType<Connector>();
    }
    public void SetupServerParams(string serverName, string ip)
    {
        serverText.text = serverName;
        ipAdresText.text = ip;
    }
    public void OnConnectButtonClick()
    {
        connector.Join(ipAdresText.text);
    }
}
