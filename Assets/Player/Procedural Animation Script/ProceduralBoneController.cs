using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ProceduralBoneController : MonoBehaviour
{
    [SerializeField] MultiAimConstraint Spine;
    [SerializeField] MultiAimConstraint Head;
    [SerializeField] MultiAimConstraint RightArm;
    [SerializeField] MultiAimConstraint RightHand;
    [SerializeField] Character character;
    public GameObject sourceTarget;
    private float weight = 0;
    // Start is called before the first frame update
    void Start()
    {
        Spine.weight = 0;
        Head.weight = 0;
        RightArm.weight = 0;
        RightHand.weight = 0;
        character.Aim += SetWeight;
        character.LowWeapon += SetWeight;
    }

    // Update is called once per frame
    private void SetWeight(Weapon weapon)
    {
        weight = weapon.weapon_StanceManager.AimingWeight;
        Spine.weight = weight;
        Head.weight = weight;
        RightArm.weight = weight;
        RightHand.weight = weight;
    }
    public void SetSourceTarget(GameObject gameObject)
    {
        SetConstraintSource(Spine, gameObject);
        SetConstraintSource(Head, gameObject);
        SetConstraintSource(RightArm, gameObject);
        SetConstraintSource(RightHand, gameObject);
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
   

}
