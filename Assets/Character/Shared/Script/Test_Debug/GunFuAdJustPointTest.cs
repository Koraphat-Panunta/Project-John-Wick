using UnityEngine;

public class GunFuAdJustPointTest : MonoBehaviour
{
    [SerializeField] private GunFuExecute_Single_ScriptableObject gunFuExecute_Single_ScriptableObject;
    Vector3 attackerAdjustPosition = new Vector3();
    Vector3 attackerAdjustDir = new Vector3();

    Vector3 opponetAdjustPosition = new Vector3();
    Vector3 opponetAdjustDir = new Vector3();

    [SerializeField] private Transform attackerTransform;
    [SerializeField] private Transform opponentTransform;

    Vector3 dir => (opponentTransform.position - attackerTransform.position).normalized;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        attackerAdjustPosition 
            = attackerTransform.position 
            + (dir * gunFuExecute_Single_ScriptableObject.playerForwardRelativePosition)
            + (Vector3.Cross(dir,Vector3.down) * gunFuExecute_Single_ScriptableObject.playerRightwardRelativePosition);

        Quaternion attackerRotation
            = Quaternion.LookRotation(dir,Vector3.up)
            * Quaternion.Euler(0, gunFuExecute_Single_ScriptableObject.playerRotationRelative, 0);

        attackerAdjustDir = attackerRotation * Vector3.forward;

        opponetAdjustPosition
           = attackerTransform.position
           + (dir * gunFuExecute_Single_ScriptableObject.opponentForwardRelative)
           + (Vector3.Cross(dir,Vector3.down) * gunFuExecute_Single_ScriptableObject.opponentRightwardRelative);

        Quaternion opponentRotation
            = Quaternion.LookRotation(dir, Vector3.up)
            * Quaternion.Euler(0, gunFuExecute_Single_ScriptableObject.opponentRotationRelative, 0);

        opponetAdjustDir = opponentRotation * Vector3.forward;

        attackerAdjustDir = attackerRotation * Vector3.forward;
    }
    private void OnDrawGizmos()
    {
        // Draw attacker point and direction
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(attackerAdjustPosition, 0.2f);
        Gizmos.DrawLine(attackerAdjustPosition, attackerAdjustPosition + attackerAdjustDir);

        // Draw opponent point and direction
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(opponetAdjustPosition, 0.2f);
        Gizmos.DrawLine(opponetAdjustPosition, opponetAdjustPosition + opponetAdjustDir);
    }
}
