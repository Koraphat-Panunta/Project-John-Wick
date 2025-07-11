using UnityEngine;

public class GunFuExecuteOnGroundDebugVisualized : MonoBehaviour
{
    [SerializeField] GunFuExecute_OnGround_Single_ScriptableObject gunFuExecute_OnGround_Single_ScriptableObject;
    [SerializeField] Transform hipTransform;
    private void OnDrawGizmos()
    {
        Vector3 anchorPos = transform.position;
        Vector3 anChorDir = new Vector3(hipTransform.up.x, 0, hipTransform.up.z).normalized;

        Quaternion adjustDri = Quaternion.LookRotation(anChorDir,Vector3.up)
            *Quaternion.Euler(0,gunFuExecute_OnGround_Single_ScriptableObject.playerRotationRelative,0);
        Vector3 adjustDirVector = adjustDri * Vector3.forward;
        Vector3 adjustPos = anchorPos
            + (anChorDir * gunFuExecute_OnGround_Single_ScriptableObject.playerForwardRelativePosition)
            + (Vector3.Cross(anChorDir, Vector3.down) * gunFuExecute_OnGround_Single_ScriptableObject.playerRightwardRelativePosition);

        //Anchor Direction
        Gizmos.color = Color.green;
        Gizmos.DrawLine(anchorPos,anchorPos + anChorDir*3);

        //Anchor Position
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(adjustPos, .2f);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(adjustPos,adjustPos + (adjustDirVector*2));

    }
}
