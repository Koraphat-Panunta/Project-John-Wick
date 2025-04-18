using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public bool isOpen { get; private set; }
    public void Open()
    {
        animator.SetTrigger("Open/Close");
        isOpen = true;
    }
    public void Close()
    {
        isOpen = false;
    }


}
