using UnityEngine;

public class MainHandSocket : MonoBehaviour, IWeaponAttachingAble
{
    [SerializeField] private Character character;
    public Transform weaponAttachingAbleTransform => this.transform;
    public IWeaponAdvanceUser weaponAdvanceUser => character.GetComponent<IWeaponAdvanceUser>();

    public IAttacher curAttacher { get; set; }
    public Transform attachAbleTransform { get ; set ; }
    IAttacher IAttachAble.curAttacher { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public T GetCurrentAttacher<T>() where T : IAttacher
    {
        throw new System.NotImplementedException();
    }

    private void OnValidate()
    {
        if(character == null)
            character = GetComponentInParent<Character>();
    }
}
