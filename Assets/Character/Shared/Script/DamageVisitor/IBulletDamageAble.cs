using UnityEngine;
using static SubjectPlayer;

public interface IBulletDamageAble : IDamageAble
{
   public float penatrateResistance { get; }
   public void TakeDamage(IDamageVisitor damageVisitor,Vector3 hitPos,Vector3 hitDir,float hitforce);
}
