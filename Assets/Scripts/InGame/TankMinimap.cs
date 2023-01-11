using Mirror;
using UnityEngine;

public class TankMinimap : NetworkBehaviour
{
    [SerializeField] private SpriteRenderer currentIcon;
    [SerializeField] private Sprite whiteTankIcon;


    public override void OnStartAuthority()
    {
        currentIcon.sprite = whiteTankIcon;
    }
}
