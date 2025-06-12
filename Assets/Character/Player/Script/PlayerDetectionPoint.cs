using UnityEngine;

public class PlayerDetectionPoint : MonoBehaviour,I_NPCTargetAble
{
    [SerializeField] private Player player;
    public Character selfNPCTarget => player.selfNPCTarget;
}
