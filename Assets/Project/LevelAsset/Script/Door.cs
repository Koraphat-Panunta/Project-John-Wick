using UnityEngine;

public class Door : MonoBehaviour,I_Interactable
{
    [SerializeField] private Animator animator;
    public bool isOpen { get; private set; }
    public Collider _collider { get => this.collider; set => this.collider = value; }
    [SerializeField] private Collider collider;
    public bool isBeenInteractAble { get ; set ; }

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

    public void DoInteract(I_Interacter i_Interacter)
    {
        if(isOpen)
            Close();
        else
            Open();
    }
}
