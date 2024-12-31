using UnityEngine;

public class CombatOffensiveInstinct:IEnvironmentAware
{
    public float offensiveIntensity;
    private FieldOfView fieldOfView;
    private GameObject target;
    private ICombatOffensiveInstinct offensiveInstinct;

    public CombatOffensiveInstinct(FieldOfView fieldOfView,ICombatOffensiveInstinct OffensiveInstincted,Environment environment)
    {
        this.fieldOfView = fieldOfView;
        offensiveInstinct = OffensiveInstincted;
        environment.Add_Listener(this);
    }

    public void UpdateSening()
    {
        TargetPointingWeaponAware();
        float coolDownOffensiveIntensity = 10;

        if(offensiveIntensity >0)
        offensiveIntensity -= coolDownOffensiveIntensity*Time.deltaTime;
    }

    public void OnAware(GameObject sourceFrom, EnvironmentType environmentType)
    {
        if(environmentType != EnvironmentType.Sound)
            return;

        if (sourceFrom.TryGetComponent<Player>(out Player player) == false)
            return ;

            Vector3 shootingLine = player.pointingPos - player.currentWeapon.bulletSpawnerPos.position;
            Vector3 referencePoint = offensiveInstinct.objInstict.transform.position + new Vector3(0,1,0);
        if (IsLineOfSightCloseEnough(shootingLine,
                referencePoint,
                player.currentWeapon.bulletSpawnerPos.position))
        {
            IncreseBulletSuppressIntensity();
        }
    }
    private bool IsLineOfSightCloseEnough(Vector3 bulletLine,Vector3 referencePos,Vector3 startPos)
    {
        float raduisAware = 3;

        Vector3 startPosToRefPoint = referencePos - startPos;
        float t = Vector3.Dot(startPosToRefPoint, bulletLine) / Vector3.Dot(bulletLine,bulletLine);
        if (t >1||t<0)
            return false;
       


        Vector3 closestPoint = startPos + t*bulletLine;
        Debug.DrawLine(closestPoint, referencePos,Color.yellow);
        Debug.DrawLine(startPos, startPos + bulletLine, Color.red);

        if(Vector3.Distance(closestPoint,referencePos) < raduisAware)
            return true;

        return false;
    }
    private void IncreseBulletSuppressIntensity()
    {
        float bulletsupressIntensity = 25;

        offensiveIntensity += bulletsupressIntensity;
    }
    private void TargetPointingWeaponAware()
    {
        float lineOfsightAwareIntensity = 20;

        if(fieldOfView.FindSingleObjectInView(offensiveInstinct.targetLayer,new Vector3(0, 1.23f, 0),out GameObject target) == false)
        return;
        Debug.Log("Find Target");
        if(target.TryGetComponent<IWeaponAdvanceUser>(out IWeaponAdvanceUser thisTarget))
        {
            Debug.Log("Out Target");
            if (thisTarget.isAiming == false)
                return ;
            Vector3 aimingLine = thisTarget.pointingPos - thisTarget.currentWeapon.bulletSpawnerPos.position;
            Vector3 referencePos = offensiveInstinct.objInstict.transform.position + new Vector3(0,1,0);
            Vector3 startPos = thisTarget.currentWeapon.bulletSpawnerPos.position;

            if(IsLineOfSightCloseEnough(aimingLine,referencePos,startPos))
            {
                offensiveIntensity += lineOfsightAwareIntensity*Time.deltaTime;
            }
        }
    }
}
