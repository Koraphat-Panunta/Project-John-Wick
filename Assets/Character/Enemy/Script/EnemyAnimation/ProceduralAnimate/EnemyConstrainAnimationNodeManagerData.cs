using UnityEngine;
using UnityEngine.Animations.Rigging;

public partial class EnemyConstrainAnimationNodeManager
{
    [Header("── Constraint Components ──────────────────────────────────")]
    public BodyLookConstrainManager bodyLookConstrainManager;
    public HandArmIKConstraintManager leftHandIKConstraint;
    public HandArmIKConstraintManager rightHandIKConstraint;
    public LegsConstrainManager legsConstrainManager;

    [Header("── Rig ──────────────────────────────────────────────────────")]
    [SerializeField] private Rig rig;

    [Header("── Body ADS ScriptableObjects ──────────────────────────────")]
    [SerializeField] private AimBodyConstrainScriptableObject painStateBodyConstraintSCRP;
    [SerializeField] private AimBodyConstrainScriptableObject primaryAimSplineLookConstrainScriptableObject;
    [SerializeField] private AimBodyConstrainScriptableObject secondaryAimSplineLookConstrainScriptableObject;

    [Header("── Arm Pain State ScriptableObjects ─────────────────────────")]
    [SerializeField] private TransformOffsetSCRP armAnchorSwingOffsetPosition;
    [SerializeField] private TransformOffsetSCRP armBalancePointOffset;
    [SerializeField] private AnimationCurve painBodyRespondCurve;

    [Header("── Legs ScriptableObjects ───────────────────────────────────")]
    [SerializeField] private ProceduralLegsWalkConstrainSCRP proceduralLegsPainStateWalkConstrainSCRP;
}
