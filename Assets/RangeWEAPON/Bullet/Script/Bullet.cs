
using UnityEngine;
using System;


public abstract class Bullet:
    IHPDamageVisitor
    ,IPostureDamageVisitor
    ,INoiseMakingAble
{

    public abstract float _hPDamage { get; set; }
    public abstract float _postureDamageVisitor { get; set; }
    public abstract float _pureDestructionDamage { get; set; }
    public virtual float _headShotDamageMultiply { get => 1; }
    public virtual float GetHpDamage { get => _hPDamage * (penetrateRate/maxPenetrateRate);  }
    public float GetPostureDamage { get => _postureDamageVisitor * (penetrateRate / maxPenetrateRate); }
    public float GetDestructionDamage { get => _pureDestructionDamage * (penetrateRate / maxPenetrateRate); }
    public virtual float maxPenetrateRate { get => 1; }
    public float penetrateRate { get;private set; }
    protected virtual float bulletHitForce { get; set; }

    public static LayerMask hitLayer = LayerMask.GetMask("Default", "BodyPart", "Ground", "Player");
    protected static readonly int _bodyPartMask = LayerMask.GetMask("BodyPart");
    protected const float MAX_DISTANCE = 350;
    public RangeWeapon weapon { get; protected set; }
    public Vector3 position { get => weapon.bulletSpawner.transform.position; set { } }
    public NoiseMakingBehavior noiseMakingBehavior { get ; set ; }
    public Action<Collider, Vector3, Vector3> bulletHitNotify;
    public abstract BulletType myType { get; protected set; }
    public Bullet(RangeWeapon weapon)
    {
        bulletHitForce = 5;
        this.weapon = weapon;
        noiseMakingBehavior = new NoiseMakingBehavior(this);

    }
    private readonly float maxAngle = 45f;
    public virtual Vector3 Shoot(BulletSpawner bulletSpawner, Vector3 pointPos)
    {
        // Normalize input
        Vector3 dirToPoint = (pointPos - bulletSpawner.transform.position).normalized;

        // Basis: forward, right, up
        Vector3 fwd = bulletSpawner.transform.forward.normalized;
        Vector3 right = Vector3.Cross(Vector3.up, fwd).normalized;
        Vector3 up = Vector3.Cross(fwd, right).normalized;

        // Project onto local basis (dot products give angles)
        float horizontalAngle = Mathf.Atan2(Vector3.Dot(dirToPoint, right), Vector3.Dot(dirToPoint, fwd)) * Mathf.Rad2Deg;
        float verticalAngle = (Mathf.Atan2(Vector3.Dot(dirToPoint, up), Vector3.Dot(dirToPoint, fwd)) * Mathf.Rad2Deg) * -1;


        // Clamp angles
        horizontalAngle = Mathf.Clamp(horizontalAngle, - this.maxAngle, this.maxAngle);
        verticalAngle = Mathf.Clamp(verticalAngle, -this.maxAngle, this.maxAngle);

        // Rebuild direction from clamped angles
        Quaternion rot = Quaternion.AngleAxis(horizontalAngle, Vector3.up) *
                         Quaternion.AngleAxis(verticalAngle, right);
        Vector3 clampedDir = (rot * fwd).normalized;

        noiseMakingBehavior.VisitAllHeardingAbleInRaduis(19, _bodyPartMask);

        // Calculate and apply impulse force
        Vector3 force = clampedDir;
        Vector3 rayDir = clampedDir;
        Ray ray = new Ray(bulletSpawner.transform.position,rayDir);

        

        RaycastHit[] raycastHits = Physics.SphereCastAll(ray, 0.015f, MAX_DISTANCE, hitLayer, QueryTriggerInteraction.Ignore);

        Debug.DrawRay(bulletSpawner.transform.position, rayDir * MAX_DISTANCE, Color.yellow,5);

        if (raycastHits.Length > 0)
        {
            System.Array.Sort(raycastHits, (a, b) => a.distance.CompareTo(b.distance));
            HitExecute(raycastHits, rayDir,out RaycastHit lastHit);
            return lastHit.point;
        }
        else
            return ray.GetPoint(MAX_DISTANCE);


    }
    protected virtual void HitExecute(RaycastHit[] rayCastHits,Vector3 dir,out RaycastHit lastHit)
    {
        this.penetrateRate = maxPenetrateRate;
        lastHit = new RaycastHit();

        for (int i = 0;i< rayCastHits.Length ; i++) 
        {
            lastHit = rayCastHits[i];
            if (rayCastHits[i].collider.TryGetComponent<IBulletDamageAble>(out IBulletDamageAble bulletDamageAble))
            {
                //Debug.Log("bullet raycast hit = " + rayCastHits[i].collider.gameObject);
                //Debug.Log("panetrateRate = " + this.penetrateRate);
                if(bulletDamageAble == (weapon.userWeapon as IBulletDamageAble))
                {
                    Debug.Log("Ignore self damaged");
                    continue;
                }

                bulletDamageAble.TakeDamageBullet(this, rayCastHits[i].point,dir,bulletHitForce);
                if(bulletHitNotify!= null)
                bulletHitNotify.Invoke(rayCastHits[i].collider, rayCastHits[i].point,dir);
                penetrateRate -= bulletDamageAble.penatrateResistance;

                if(penetrateRate <= 0)
                    break;
            }
            else
                break;
        }
        this.penetrateRate = maxPenetrateRate;
    }

    public void OnNotifyFeedBackVisitor(IDamageAble damageAble)
    {
        if(this.weapon.userWeapon != null 
            && this.weapon.userWeapon is IDamageVisitor damageVisitor)
        {
            damageVisitor.OnNotifyFeedBackVisitor(damageAble);
        }
    }
}
