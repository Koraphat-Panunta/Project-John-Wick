using UnityEngine;

public partial class Enemy : IBulletDamageAble
{
    public IBulletDamageAble bulletDamageAbleBodyPartBehavior { get; set; }
    public float penatrateResistance => 1;
}
