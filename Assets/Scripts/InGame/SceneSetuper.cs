using Mirror;
using UnityEngine;

public class SceneSetuper : NetworkBehaviour
{
    [SerializeField] private GameObject terrain;

    public override void OnStartClient()
    {
        Instantiate(terrain);
        base.OnStartClient();
    }
}
