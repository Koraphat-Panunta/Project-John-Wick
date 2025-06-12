using UnityEngine;

[RequireComponent(typeof(AmmoGetAbleObject))]
public class AmmoRecievedAbleItemHeadUpDisplay : HeadUpDisplayItem
{
    [SerializeField] AmmoGetAbleObject m_Ammo;
    protected override string textShow { get => "AMMO" ; set { } }

   
}
