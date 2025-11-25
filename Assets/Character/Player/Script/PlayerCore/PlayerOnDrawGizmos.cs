using UnityEngine;

public partial class Player 
{
    private void OnDrawGizmos()
    {
        if(Application.IsPlaying(this) == false)
            return;
      
        //this.DrawGizmosGunFuTarget();
        this.DrawGizmosInteractAble();

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, .2f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.forward);

    }
    private void DrawGizmosGunFuTarget()
    {
        if (attackedAbleGunFu != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(attackedAbleGunFu._character.transform.position, .15f);
        }

        if (executedAbleGunFu != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(executedAbleGunFu._character.transform.position, .15f);
        }
    }
    private void DrawGizmosInteractAble()
    {
        Vector3 castPos = centreTransform.position;
        Vector3 castDir = (weaponAdvanceUser._pointingPos - castPos).normalized;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(castPos, castPos + (castDir*interacter_distaceDetect));

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(castPos + (castDir * interacter_distaceDetect), interacter_sphereCastDetect);

    }
}
