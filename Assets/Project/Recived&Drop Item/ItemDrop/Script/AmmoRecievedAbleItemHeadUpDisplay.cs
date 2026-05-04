using UnityEngine;

[RequireComponent(typeof(Pickupable))]
public class AmmoRecievedAbleItemHeadUpDisplay : HeadUpDisplayItem
{
    protected override string textShow { get => "AMMO"; set { } }
}
