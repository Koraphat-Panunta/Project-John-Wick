using System;
using UnityEngine;

public class GunFuHitNodeLeaf : PlayerStateNodeLeaf, IGunFuNode
{
    public float _timer { get; set; }
    public IGunFuAble gunFuAble { get; set; }
    public IGotGunFuAttackedAble gotGunFuAttackedAble { get; set; }
    public AnimationClip _animationClip { get; set; }
    public GunFuHitNodeLeaf(Player player, Func<bool> preCondition) : base(player, preCondition)
    {

    }

  
}
