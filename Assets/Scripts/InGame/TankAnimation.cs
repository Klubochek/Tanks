using Mirror;
using Tanks.Input;
using UnityEngine;

public class TankAnimation : NetworkBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private TankController _tankController;
    [SerializeField] private TowerController _towerController;

    public void PlayDestroyAnimation()
    {
        _tankController.enabled = false;
        _towerController.enabled = false;


        CmdPlayDeathAnimation();
    }
    [Command]
    private void CmdPlayDeathAnimation()
    {
        RpcPlayDeathAnimation();
    }
    [ClientRpc]
    private void RpcPlayDeathAnimation()
    {
        _animator.enabled = true;
    }
}
