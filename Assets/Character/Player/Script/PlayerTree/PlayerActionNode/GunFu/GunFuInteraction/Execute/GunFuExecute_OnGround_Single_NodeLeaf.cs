using System;
using UnityEngine;

public class GunFuExecute_OnGround_Single_NodeLeaf : GunFuExecute_Single_NodeLeaf
{
    public GunFuExecute_OnGround_Single_NodeLeaf(Player player, Func<bool> preCondition, GunFuExecute_Single_ScriptableObject gunFuExecute_Single_ScriptableObject) : base(player, preCondition, gunFuExecute_Single_ScriptableObject)
    {
    }
}

