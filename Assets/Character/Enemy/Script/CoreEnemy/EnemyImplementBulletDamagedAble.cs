using UnityEngine;

public partial class Enemy : IBulletDamageAble
{
    public float penatrateResistance => 1;

    public void TakeDamageBullet(Bullet bulletObj, Vector3 hitPos, Vector3 hitDir, float hitforce)
    {
        
    }
}
