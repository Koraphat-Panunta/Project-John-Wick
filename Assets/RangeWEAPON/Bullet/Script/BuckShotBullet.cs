using UnityEngine;

public class BuckShotBullet : Bullet
{
    private const int PELLET_COUNT = 8;
    private const float SPREAD_ANGLE = 10f;

    public override BulletType myType { get; protected set; }
    public override float _hPDamage { get => weapon.weaponStatsScriptableObject._hpDamage / PELLET_COUNT; set { } }
    public override float _postureDamageVisitor { get => weapon.weaponStatsScriptableObject._postureDamage / PELLET_COUNT; set { } }
    public override float _pureDestructionDamage { get => weapon.weaponStatsScriptableObject._destructionDamage / PELLET_COUNT; set { } }

    public BuckShotBullet(RangeWeapon weapon) : base(weapon)
    {
        myType = BulletType.buckShotAmmo;
    }

    public override Vector3 Shoot(BulletSpawner bulletSpawner, Vector3 pointPos)
    {
        noiseMakingBehavior.VisitAllHeardingAbleInRaduis(19, _bodyPartMask);

        for (int i = 0; i < PELLET_COUNT; i++)
        {
            Vector3 hitPos = ShootPellet(bulletSpawner);
            Debug.DrawLine(this.position, hitPos, Color.red, 3);
            bulletSpawner.StartCoroutine(bulletSpawner.SpawnTrail(bulletSpawner.transform.position, hitPos, bulletSpawner.bulletTrail));
        }

        // Return start position so BulletSpawner's direction-check (> .9) evaluates to 0 and skips its own trail
        return bulletSpawner.transform.position;
    }

    private Vector3 ShootPellet(BulletSpawner bulletSpawner)
    {
        Vector3 pelletDir = GetSpreadDirection(bulletSpawner.transform);
        Ray ray = new Ray(bulletSpawner.transform.position, pelletDir);

        //Debug.DrawRay(bulletSpawner.transform.position, pelletDir,Color.red,4);

        RaycastHit[] hits = Physics.SphereCastAll(ray, 0.015f, MAX_DISTANCE, hitLayer, QueryTriggerInteraction.Ignore);

        if (hits.Length > 0)
        {
            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
            HitExecute(hits, pelletDir, out RaycastHit lastHit);
            return lastHit.point;
        }
        return ray.GetPoint(MAX_DISTANCE);
    }

    // Uniform random direction within a cone using a disk projection
    private Vector3 GetSpreadDirection(Transform spawner)
    {
        Vector2 disk = Random.insideUnitCircle * Mathf.Tan(SPREAD_ANGLE * Mathf.Deg2Rad);
        Vector3 localDir = new Vector3(disk.x, disk.y, 1f);
        return spawner.TransformDirection(localDir.normalized);
    }
}
