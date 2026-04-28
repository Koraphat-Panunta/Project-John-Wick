using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;

public class EnemySetUp : MonoBehaviour, IInitializedAble
{
    [SerializeField] BodySetup bodySetup;

    [SerializeField] public LocalServiceLocator localServiceLocator;
    [SerializeField] public Enemy enemy;
    [SerializeField] public DynamicCapsuleCollider dynamicCapsuleCollider;

    [SerializeField] ParentConstraint mainHandSocket;
    [SerializeField] ParentConstraint secondHandSocket;
    [SerializeField] ParentConstraint primaryWeaponSocket;
    [SerializeField] ParentConstraint secondaryWeaponSocket;
    public void SetUp()
    {
        this.dynamicCapsuleCollider.transformPoints = new Transform[6];

        this.dynamicCapsuleCollider.transformPoints[0] = this.bodySetup.humanoidBone._top_head_bone;//Top_Head_Bone
        this.dynamicCapsuleCollider.transformPoints[1] = this.bodySetup.humanoidBone._leftShoulderBone;
        this.dynamicCapsuleCollider.transformPoints[2] = this.bodySetup.humanoidBone._rightShoulderBone;
        this.dynamicCapsuleCollider.transformPoints[3] = this.bodySetup.humanoidBone._leftFootBone;
        this.dynamicCapsuleCollider.transformPoints[4] = this.bodySetup.humanoidBone._rightFootBone;
        this.dynamicCapsuleCollider.transformPoints[5] = this.enemy.transform;

        SaveEditorChanged.SaveEditorChangedObject(this.dynamicCapsuleCollider);

        this.enemy.humanoidBone = this.bodySetup.humanoidBone;
        this.enemy.animator = this.bodySetup.enemyAnimationManager.animator;

        SaveEditorChanged.SaveEditorChangedObject(this.enemy);


        this.mainHandSocket.SetSource(0, new ConstraintSource { sourceTransform = this.bodySetup.humanoidBone._rightHandBone,weight = 1 });
        this.secondHandSocket.SetSource(0, new ConstraintSource { sourceTransform = this.bodySetup.humanoidBone._leftHandBone, weight = 1 });
        this.primaryWeaponSocket.SetSource(0, new ConstraintSource { sourceTransform = this.bodySetup.humanoidBone._spine_0_Bone, weight = 1 });
        this.secondaryWeaponSocket.SetSource(0, new ConstraintSource { sourceTransform = this.bodySetup.humanoidBone.hips, weight = 1 });
    }

    public void Initialized()
    {
        this.RegisterLocalServiceLocator();
    }
    private void RegisterLocalServiceLocator()
    {
        this.localServiceLocator.Register<FullBodyCharacterPart>(this.bodySetup.fullBodyCharacterPart);
        this.localServiceLocator.Register<HumanoidBone>(this.bodySetup.humanoidBone);
        this.localServiceLocator.Register<Animator>(this.bodySetup.enemyAnimationManager.animator);

    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (!Application.isPlaying)
        {
            if(this.localServiceLocator == null)
            {
                TryGetComponent<LocalServiceLocator>(out LocalServiceLocator localServiceLocator);
                this.localServiceLocator = localServiceLocator;
            }
            if(this.enemy == null)
            {
                TryGetComponent<Enemy>(out Enemy enemy);
                this.enemy = enemy;
            }
            if(this.dynamicCapsuleCollider == null)
            {
                TryGetComponent<DynamicCapsuleCollider>(out DynamicCapsuleCollider dynamicCapsuleCollider);
                this.dynamicCapsuleCollider = dynamicCapsuleCollider;
            }

        }
#endif
    }
}
[CustomEditor(typeof(EnemySetUp))]
public class EnemySetUpEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("SetUpEnemy"))
        {
            ((EnemySetUp)target).SetUp();
        }

        DrawDefaultInspector();
    }
}
