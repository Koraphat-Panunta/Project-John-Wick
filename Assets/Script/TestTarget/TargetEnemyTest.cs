using UnityEngine;

public class TargetEnemyTest : MonoBehaviour, IGunFuGotAttackedAble
{
    public bool _triggerHitedGunFu { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public Transform _gunFuAttackedAble { get => transform; set => throw new System.NotImplementedException(); }
    public Vector3 attackedPos { get => transform.position; set => throw new System.NotImplementedException(); }
    public IGunFuNode curAttackerGunFuNode { get; set ; }
    public INodeLeaf curNodeLeaf { get ; set ; }
    public IGunFuAble gunFuAbleAttacker { get; set; }
    public IMovementCompoent _movementCompoent { get; set; }
    public IWeaponAdvanceUser _weaponAdvanceUser { get; set; }
    public IDamageAble _damageAble { get; set; }
    public bool _isDead { get; set; }
    public bool _isGotAttackedAble { get; set; }
    public bool _isGotExecutedAble { get; set; }

    [SerializeField, TextArea] private string TargetEnemyTestDebug;

    private void Awake()
    {
        _isGotAttackedAble = true;
    }

    public void TakeGunFuAttacked(IGunFuNode gunFu_NodeLeaf, IGunFuAble gunFuAble)
    {
       if(gunFu_NodeLeaf is EnemySpinKickGunFuNodeLeaf)
        {
            TargetEnemyTestDebug += "Got enemySpinKickGunFuNodeLeaf";
        }
    }
}
