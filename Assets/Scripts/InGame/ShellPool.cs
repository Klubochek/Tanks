using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class ShellPool : NetworkBehaviour
{
    [SerializeField] private GameObject shellPrefab;
    [SerializeField] private Transform poolTransform;

    [SerializeField] public List<GameObject> shellPool = new List<GameObject>();
}