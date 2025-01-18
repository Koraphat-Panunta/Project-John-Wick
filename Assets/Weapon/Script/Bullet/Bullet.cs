using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet:IDamageVisitor
{
    public abstract float hpDamage { get; set; }
    public abstract float impactDamage { get; set; }
    public abstract float recoilKickBack { get; set; }

    protected LayerMask hitLayer;
    protected const float MAX_DISTANCE = 1000;
    public abstract BulletType myType { get; set; } 

    public Bullet()
    {
       
    }
    public virtual void ShootDirection(Vector3 spawnerPosition,Vector3 pointPos)
    {
        int DefaultMask = LayerMask.GetMask("Default");
        int BodyPartMask = LayerMask.GetMask("Enemy");
        int PlayerHitMask = LayerMask.GetMask("Player");
        hitLayer = DefaultMask + BodyPartMask + PlayerHitMask;
        // Calculate and apply impulse force
        Vector3 force = (pointPos-spawnerPosition).normalized;
        Vector3 rayDir = (pointPos - spawnerPosition).normalized;
        Ray ray = new Ray(spawnerPosition,rayDir);
        if (Physics.Raycast(ray,out RaycastHit hit,MAX_DISTANCE,hitLayer))
        {
            HitExecute(hit);
        }
    }
    protected virtual void HitExecute(RaycastHit hit)
    {
        Collider collider = hit.collider;
        if(collider.TryGetComponent<IBulletDamageAble>(out IBulletDamageAble damageAble))
        {
            damageAble.TakeDamage(this,hit.point);
        }
        //if (collider.TryGetComponent<BodyPart>(out BodyPart bodyPart))
        //    bodyPart.GotHit(hpDamage);

        //if (collider.TryGetComponent<Player>(out Player playerAnimationManager))
        //    playerAnimationManager.TakeDamage(hpDamage);

    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.collider.TryGetComponent<BodyPart>(out BodyPart bodyPart))
    //    {
    //        bodyPart.GotHit(damage);
    //    }
    //    if(collision.collider.TryGetComponent<Player>(out Player playerAnimationManager))
    //    {
    //        playerAnimationManager.TakeDamage(damage);
    //    }
    //    DrawBulletLine.bulletHitPos.Add(gameObject.transform.position);
    //    Destroy(gameObject);
    //}
    //private void OnDrawGizmos()
    //{
        
    //}
}
