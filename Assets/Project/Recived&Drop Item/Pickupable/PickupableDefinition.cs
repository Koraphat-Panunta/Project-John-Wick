using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Designer-facing item definition. Carries the data (id, name, icon) and a
/// polymorphic list of effects via [SerializeReference]. One asset = one item type.
/// Multiple effects = "MedKit" with HP + temporary power-up, etc.
/// </summary>
[CreateAssetMenu(fileName = "Pickupable", menuName = "ScriptableObjects/Pickupable")]
public class PickupableDefinition : ScriptableObject
{
    public string id;
    public string displayName;
    public Sprite icon;

    [Tooltip("Optional prefab spawned when the Dropper drops this item. Must have a Pickupable component.")]
    public GameObject worldPrefab;

    [Tooltip("Polymorphic list of pickup effects. Right-click to Add: HpEffect, AmmoEffect, PowerUpAppliedEffect.")]
    [SerializeReference] public List<IPickupEffect> effects = new List<IPickupEffect>();
}
