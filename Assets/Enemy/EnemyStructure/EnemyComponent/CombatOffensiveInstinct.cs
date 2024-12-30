using UnityEngine;

public class CombatOffensiveInstinct:IEnvironmentAware
{
    public float offensiveIntensity;
    private FieldOfView fieldOfView;
    private GameObject target;
    private ICombatOffensiveInstinct offensiveInstinct;

    public CombatOffensiveInstinct(FieldOfView fieldOfView,ICombatOffensiveInstinct OffensiveInstincted)
    {
        this.fieldOfView = fieldOfView;
        offensiveInstinct = OffensiveInstincted;
    }

    public void UpdateSening()
    {
        float coolDownOffensiveIntensity = 10;
        //if(this.fieldOfView.)
        offensiveIntensity -= coolDownOffensiveIntensity*Time.deltaTime;
    }

    public void OnAware(GameObject sourceFrom, EnvironmentType environmentType)
    {
        if(environmentType != EnvironmentType.Sound)
            return;

        if (sourceFrom.TryGetComponent<Player>(out Player player) == false)
            return ;

            Vector3 shootingLine = player.pointingPos - player.currentWeapon.bulletSpawnerPos.position;
            Vector3 referencePoint = offensiveInstinct.objInstict.transform.position;
        if (IsBulletLineCloseEnough(shootingLine,
                referencePoint,
                player.currentWeapon.bulletSpawnerPos.position))
        {
            IncreseBulletSuppressIntensity();
        }
    }
    private bool IsBulletLineCloseEnough(Vector3 bulletLine,Vector3 referencePos,Vector3 startPos)
    {
        float raduisAware = 3;

        Vector3 startPosToRefPoint = referencePos - startPos;
        float t = Vector3.Dot(startPosToRefPoint, bulletLine) / 1;

        Vector3 closestPoint = startPos + t*bulletLine;
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
        if(fieldOfView.FindSingleObjectInView(offensiveInstinct.targetLayer,new Vector3(0, 1.23f, 0),out GameObject target) == false)
        return;

        if(target.TryGetComponent<IWeaponAdvanceUser>(out IWeaponAdvanceUser thisTarget))
        {
            //if(thisTarget.isA)
        }
    }
}
