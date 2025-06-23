using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Player))]
public class DynamicColliderPlayerUpdate : MonoBehaviour,IObserverPlayer
{
    private CapsuleCollider capsuleCollider;

    [SerializeField] private float StandCenterY;
    [SerializeField] private float StandHeight;

    [SerializeField] private float CrouchCenterY;
    [SerializeField] private float CrouchHeight;
    public void OnNotify(Player player, SubjectPlayer.NotifyEvent playerAction)
    {
    }
    public void OnNotify<T>(Player player, T node) where T : INode
    {
        if (node is PlayerStateNodeLeaf playerStateNodeLeaf)
            switch (playerStateNodeLeaf)
            {
                case PlayerCrouch_Idle_NodeLeaf playerCrouchIdleNodeLeaf:
                case PlayerCrouch_Move_NodeLeaf playerCrouchMoveNodeLeaf:
                    {
                        if ((player._weaponManuverManager as INodeManager).TryGetCurNodeLeaf<AimDownSightWeaponManuverNodeLeaf>())
                        {
                            capsuleCollider.center = new Vector3(capsuleCollider.center.x, StandCenterY, capsuleCollider.center.z);
                            capsuleCollider.height = StandHeight;
                        }
                        else
                        {
                            capsuleCollider.center = new Vector3(capsuleCollider.center.x, CrouchCenterY, capsuleCollider.center.z);
                            capsuleCollider.height = CrouchHeight;
                        }
                        break;
                    }
                case PlayerDodgeRollStateNodeLeaf playerDodgeRollStateNodeLeaf:
                    {
                        capsuleCollider.center = new Vector3(capsuleCollider.center.x, CrouchCenterY, capsuleCollider.center.z);
                        capsuleCollider.height = CrouchHeight;
                        break;
                    }
                case PlayerStandMoveNodeLeaf playerStandMoveNodeLeaf:
                case PlayerStandIdleNodeLeaf playerStandIdleNodeLeaf:
                    {
                        capsuleCollider.center = new Vector3(capsuleCollider.center.x, StandCenterY, capsuleCollider.center.z);
                        capsuleCollider.height = StandHeight;
                        break;
                    }

            }
    }

    void Start()
    {
        GetComponent<Player>().AddObserver(this);
        this.capsuleCollider = GetComponent<CapsuleCollider>();

        StandCenterY = capsuleCollider.center.y;
        StandHeight = capsuleCollider.height;
    }

   
}
