using Mirror;
using System.Collections;
using System.Collections.Generic;
using Tanks.Input;
using UnityEngine;

public class TankAnimation : NetworkBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private TankController tankController;
    [SerializeField] private TowerController towerController;
    [SerializeField] private PlayerCameraController playerCamera;

    public void PlayDestroyAnimation()
    {
        tankController.enabled = false;
        towerController.enabled = false;
        //playerCamera.enabled = false;
        

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
        animator.enabled = true;
    }
}
