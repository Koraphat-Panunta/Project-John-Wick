using UnityEngine;

public class MeleeWeapon : MonoBehaviour
    ,IObjectGrabbedAble
    ,IHPDamageVisitor
{
    public Transform grabSocketTransform => throw new System.NotImplementedException();

    public IGrabAbleObject currentGrabbedObject => throw new System.NotImplementedException();

    public float _hPDamage => throw new System.NotImplementedException();

    public void GrabAttach(IGrabAbleObject grabAble, Vector3 additionalOffsetPosition, Quaternion additionalOffsetRotation, float attachingDuration)
    {
        throw new System.NotImplementedException();
    }

    public void GrabDetach()
    {
        throw new System.NotImplementedException();
    }

    public void OnNotifyFeedBackVisitor(IDamageAble damageAble)
    {
        throw new System.NotImplementedException();
    }
}
