using Mirror;
using UnityEngine;

public class TankMinimap : NetworkBehaviour
{
    [SerializeField] private SpriteRenderer _currentIcon;
    [SerializeField] private Sprite _whiteTankIcon;


    public override void OnStartAuthority()
    {
        _currentIcon.sprite = _whiteTankIcon;
    }
}
