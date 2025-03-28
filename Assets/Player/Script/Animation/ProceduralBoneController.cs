using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ProceduralBoneController : MonoBehaviour,IObserverPlayer
{
    [SerializeField] RigBuilder rigBuilder;
    [SerializeField] MultiAimConstraint Spine;
    [SerializeField] MultiAimConstraint Head;
    [SerializeField] MultiAimConstraint RightArm;
    [SerializeField] MultiAimConstraint RightHand;
    [SerializeField] TwoBoneIKConstraint LeftHandIK;
    [SerializeField] Character character;
    [SerializeField] MultiRotationConstraint leanRotate;
    public GameObject sourceTarget;
    private float weight = 0;
    public Transform rotTranform;
    // Start is called before the first frame update
    void Start()
    {
        Spine.weight = 0;
        Head.weight = 0;
        RightArm.weight = 0;
        RightHand.weight = 0;
        LeftHandIK.weight = 0;

       if(character.TryGetComponent<Player>(out Player player))
        {
            player.AddObserver(this);
           
        }
    }

    // UpdateNode is called once per frame
    private void SetWeight(Weapon weapon)
    {
        if (weapon != null)
        {
            float aimingWeight = weapon.userWeapon.weaponManuverManager.aimingWeight;
            weight = aimingWeight;
            Spine.weight = weight;
            Head.weight = weight;
            //RightArm.weight = weight;
            //RightHand.weight = weight;
            //LeftHandIK.weight = weight;
        }
        else
        {
            Spine.weight = 0;
            Head.weight = 0;
        }
    }
    public void SetSourceTarget(GameObject gameObject)
    {
        SetConstraintSource(Spine, gameObject);
        SetConstraintSource(Head, gameObject);
        SetConstraintSource(RightArm, gameObject);
        SetConstraintSource(RightHand, gameObject);
        
        rigBuilder.Build();
    }
    private void SetConstraintSource(MultiAimConstraint constraint, GameObject source)
    {
        var data = constraint.data.sourceObjects;
        data.Clear();

        // Create a new constraint source
        WeightedTransform sourceTransform = new WeightedTransform(source.transform, 1.0f);
        data.Add(sourceTransform);

        // Apply the updated source objects
        constraint.data.sourceObjects = data;
    }
    
    public void OnNotify(Player player, SubjectPlayer.PlayerAction playerAction)
    {
        if(playerAction == SubjectPlayer.PlayerAction.Aim||playerAction == SubjectPlayer.PlayerAction.LowReady)
        {
            SetWeight(player._currentWeapon);
        }
        
    }

    public void OnNotify(Player player)
    {
    }
}
