using UnityEngine;

/// <summary>
/// Play-mode baking tool. Samples <see cref="clip"/> at <see cref="sampleNormalizedTime"/> onto a
/// body skeleton, reads the bones (same enumeration as IRagdollAble._bones), and writes them into
/// <see cref="output"/> so the runtime get-up node can read the pose instead of calling the
/// destructive SampleAnimation at runtime.
///
/// Skeleton source — pick ONE:
///   A) Wire <see cref="animator"/> + <see cref="humanoidBone"/> directly (baker placed on a body prefab).
///   B) Assign <see cref="bodyPrefab"/> (e.g. Enemy_Soldier_Body); the baker instantiates it at runtime,
///      bakes, then destroys it. This keeps the baker GameObject in the scene empty (no rig needed).
/// </summary>
public class PoseBoneTransformBaker : MonoBehaviour
{
    [Header("Skeleton source A: wired in scene")]
    public Animator animator;
    public HumanoidBone humanoidBone;   // for hips enumeration (same body skeleton as enemy)

    [Header("Skeleton source B: instantiate a body prefab at runtime")]
    public GameObject bodyPrefab;       // e.g. Enemy_Soldier_Body

    [Header("Bake settings")]
    public AnimationClip clip;
    [Range(0, 1)] public float sampleNormalizedTime = 0f;
    public PoseBoneTransformSCRP output;
    public bool bakeOnStart = true;

    private void Start()
    {
        if (bakeOnStart)
            Bake();
    }

    [ContextMenu("Bake")]
    public void Bake()
    {
        Animator a = animator;
        HumanoidBone hb = humanoidBone;
        GameObject spawned = null;

        // Source B: spawn the body prefab if no skeleton was wired directly.
        if ((a == null || hb == null) && bodyPrefab != null)
        {
            spawned = Instantiate(bodyPrefab);
            a = spawned.GetComponentInChildren<Animator>();
            hb = spawned.GetComponentInChildren<HumanoidBone>();
        }

        if (a == null || hb == null || clip == null || output == null)
        {
            Debug.LogError("[PoseBoneTransformBaker] Assign (animator + humanoidBone) OR bodyPrefab, plus clip and output before baking.");
            if (spawned != null) Destroy(spawned);
            return;
        }

        // Same enumeration the runtime uses for IRagdollAble._bones, so indices align.
        Transform[] bones = hb.hips.GetComponentsInChildren<Transform>();

        // Sample and read synchronously (no Animator update happens in between).
        clip.SampleAnimation(a.gameObject, sampleNormalizedTime * clip.length);

        BoneTransform[] baked = new BoneTransform[bones.Length];
        string[] names = new string[bones.Length];
        for (int i = 0; i < bones.Length; i++)
        {
            baked[i] = new BoneTransform { Position = bones[i].localPosition, Rotation = bones[i].localRotation };
            names[i] = bones[i].name;
        }

        output.sourceClip = clip;
        output.sampleNormalizedTime = sampleNormalizedTime;
        output.boneNames = names;
        output.boneLocalTransforms = baked;

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(output);
        UnityEditor.AssetDatabase.SaveAssets();   // persist the play-mode bake to the asset
#endif
        Debug.Log($"[PoseBoneTransformBaker] Baked {bones.Length} bones from '{clip.name}' into '{output.name}'.");

        if (spawned != null) Destroy(spawned);
    }
}
