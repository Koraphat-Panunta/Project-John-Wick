using UnityEngine;

[CreateAssetMenu(fileName = "ClimbParkourScriptableObject", menuName = "ScriptableObjects/Parkour/ClimbParkourScriptableObject")]
public class ClimbParkourScriptableObject : ParkourScriptableObject
{

    // Drives the warp -> climb phase switch. The "Climb" event marks the normalized time the hands
    // grip the ledge; endNormalizedTime marks when the climb finishes.
    public AnimationTriggerEventSCRP animationTriggerEventSCRP;

    // Per-component multiplier applied to the climb clip's baked root motion (Y tunes climb height,
    // XZ tunes forward reach). Tune together with the warp-destination offsets above.
    public Vector3 rootMotionScale = Vector3.one;
}
