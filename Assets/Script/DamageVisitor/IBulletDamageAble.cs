using UnityEngine;

public interface IBulletDamageAble : IDamageAble
{
   public void TakeDamage(IDamageVisitor damageVisitor,Vector3 hitPos,Vector3 hitDir,float hitforce);
}
