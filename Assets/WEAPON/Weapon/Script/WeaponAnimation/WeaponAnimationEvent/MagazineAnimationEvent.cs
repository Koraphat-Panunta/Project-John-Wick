using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MountComponent))]
public class MagazineAnimationEvent : MonoBehaviour
{
    [SerializeField] public Vector3 offsetPositionHand;
    [SerializeField] public Vector3 offsetRotationHand;

    [SerializeField] protected Weapon weapon;
    protected Transform weaponAdvanceUserHand => this.weapon.userWeapon._secondHandSocket.transform;
    [SerializeField] protected Transform weaponMagazine;

    protected MountComponent mountComponent;

    [SerializeField] protected GameObject realMagazine;

    private void OnValidate()
    {
        if(this.mountComponent == null)
            this.mountComponent = GetComponent<MountComponent>();

        if(this.mountComponent != null)
            this.mountComponent.SetAttachAbleObject(this.transform);
    }

    public void SetTransform(Transform transform)
    {
        this.transform.position = transform.position;
        this.transform.rotation = transform.rotation;
    }

    public void AttatchToMagazine(float duration)
    {
        this.gameObject.SetActive(true);
        this.mountComponent.Attach(this.weaponMagazine, Vector3.zero, Quaternion.identity,duration);
        this.StartCoroutine(CoundownEnableRealMag(duration));
    }

    public void AttatchToHand(float duration)
    {
        this.gameObject.SetActive(true);
        if(this.weaponAdvanceUserHand != null)
            this.mountComponent.Attach(this.weaponAdvanceUserHand, offsetPositionHand, Quaternion.Euler(this.offsetRotationHand), duration);
    }

    public IEnumerator CoundownEnableRealMag(float duration)
    {
        yield return new WaitForSeconds(duration);
        realMagazine.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
