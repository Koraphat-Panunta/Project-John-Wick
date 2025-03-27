using UnityEngine;

public class OpeningUICanvas : MonoBehaviour
{
    Animator animator;
    public bool isComplete { get; private set; }
    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }
    public void PlayOpeningAnimationUI()
    {
        isComplete = false;
        animator.enabled = true;
        animator.Play("OpeningAnimationUI");
    }
    public void OpeingAnimationUIIsComplete()
    {
        isComplete=true;
        animator.enabled = false;
    }
  
}

