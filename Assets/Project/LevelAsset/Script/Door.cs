using UnityEngine;

public class Door : MonoBehaviour,I_Interactable
{
    [SerializeField] private Animator animator;
    public virtual bool isOpen { get; private set; }
    public virtual Collider _collider { get => this.collider; set => this.collider = value; }
    [SerializeField]  private Collider collider;
    public virtual bool isBeenInteractAble { get ; set ; }
    [SerializeField] protected bool lockedValue;
    public virtual bool isLocked { 
        get 
        {
            if (isOpen)
            {
                lockedValue = false;
                return false;
            }
            return lockedValue;
        } 
        set 
        { 
            lockedValue = value; 
        } 
    }
    public Transform _transform { get => transform; set {} }

    private void Awake()
    {
        isBeenInteractAble = true;
    }
    public void Open()
    {
        animator.SetTrigger("DoorTrigger");
        isOpen = true;
    }
    public void Close()
    {
        animator.SetTrigger("DoorTrigger");
        isOpen = false;
    }

    public virtual void DoInteract(I_Interacter i_Interacter)
    {
        if(isLocked)
            return;

        if(isOpen)
            Close();
        else
            Open();
    }
}
