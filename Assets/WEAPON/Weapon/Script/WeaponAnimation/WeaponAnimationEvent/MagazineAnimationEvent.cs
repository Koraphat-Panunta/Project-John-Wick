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
    [SerializeField] protected Transform realWeaponMagazine;
    [SerializeField] protected Transform visibleMag;

    protected MountComponent mountComponent;

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
        this.SetActive(true);
        this.mountComponent.Attach(this.realWeaponMagazine, Vector3.zero, Quaternion.identity,duration);
        this.StartCoroutine(CoundownEnableRealMag(duration));
    }

    public void AttatchToHand(float duration)
    {
        this.SetActive(true);
        if(this.weaponAdvanceUserHand != null)
            this.mountComponent.Attach(this.weaponAdvanceUserHand, offsetPositionHand, Quaternion.Euler(this.offsetRotationHand), duration);
    }

    public IEnumerator CoundownEnableRealMag(float duration)
    {
        yield return new WaitForSeconds(duration);
        realWeaponMagazine.gameObject.SetActive(true);
        this.SetActive(false);
    }

    public void SetActive(bool active)
    {
        this.visibleMag.gameObject.SetActive(active);
    }
}
