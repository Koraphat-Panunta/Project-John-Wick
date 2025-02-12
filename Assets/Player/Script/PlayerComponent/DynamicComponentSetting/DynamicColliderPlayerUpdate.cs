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
    public void OnNotify(Player player, SubjectPlayer.PlayerAction playerAction)
    {
        if (player.playerStateNodeManager.curNodeLeaf is PlayerCrouch_Idle_NodeLeaf
            || player.playerStateNodeManager.curNodeLeaf is PlayerCrouch_Move_NodeLeaf)
        {
            switch (playerAction)
            {
                case SubjectPlayer.PlayerAction.LowReady: 
                    {
                        capsuleCollider.center = new Vector3(capsuleCollider.center.x, CrouchCenterY, capsuleCollider.center.z);
                        capsuleCollider.height = CrouchHeight;
                    }
                    break;
                case SubjectPlayer.PlayerAction.Aim:
                    {
                        capsuleCollider.center = new Vector3(capsuleCollider.center.x, StandCenterY, capsuleCollider.center.z);
                        capsuleCollider.height = StandHeight;
                    }
                    break;
            }
          
        }

        if (playerAction == SubjectPlayer.PlayerAction.StandMove
             || playerAction == SubjectPlayer.PlayerAction.StandIdle)
        {
            capsuleCollider.center = new Vector3(capsuleCollider.center.x, StandCenterY, capsuleCollider.center.z);
            capsuleCollider.height = StandHeight;
        }
    }

    public void OnNotify(Player player)
    {
        
    }

    void Start()
    {
        GetComponent<Player>().AddObserver(this);
        this.capsuleCollider = GetComponent<CapsuleCollider>();

        StandCenterY = capsuleCollider.center.y;
        StandHeight = capsuleCollider.height;
    }

   
}
