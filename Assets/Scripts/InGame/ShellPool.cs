using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class ShellPool : NetworkBehaviour
{
    [SerializeField] private GameObject shellPrefab;
    [SerializeField] private Transform poolTransform;

    [SerializeField] public List<GameObject> shellPool = new List<GameObject>();

    //public override void OnStartAuthority()
    //{
    //    CmdCreatePool();
    //}
    //[Command]
    //public void CmdCreatePool()
    //{
    //    RpcCreatePool();
    //}
    //[ClientRpc]
    //public void RpcCreatePool()
    //{
    //    for (int i = 0; i < 50; i++)
    //    {
    //        Debug.Log("Creating pool");
    //        GameObject go = Instantiate(shellPrefab, poolTransform.transform);

    //        go.name = "Shell" + i;
    //        shellPool.Add(go);
    //        go.SetActive(false);

    //    }
    //}
}