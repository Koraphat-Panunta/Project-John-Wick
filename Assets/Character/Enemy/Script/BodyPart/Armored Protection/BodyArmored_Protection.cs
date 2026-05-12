using UnityEngine;

public class BodyArmored_Protection : Armored_Protection
{
    [SerializeField] SkinnedMeshRenderer m_Renderer;
    public void Attach(BodyPart bodyPart,SkinnedMeshRenderer characterMeshRenderer)
    {
        SkinMeshBoneAssign.AssignBonesByIndex(m_Renderer, characterMeshRenderer);
        base.Attach(bodyPart);
    }
}
