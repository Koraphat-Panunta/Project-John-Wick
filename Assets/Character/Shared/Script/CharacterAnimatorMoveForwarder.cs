using UnityEngine;

public class CharacterAnimatorMoveForwarder : MonoBehaviour
{
    [SerializeField] private Character character;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        // Auto-find the Character if not wired in Inspector
        if (character == null)
            character = GetComponentInParent<Character>();
    }
    public void SetCharacter(Character character)
    {
        this.character = character;
    }
    private void OnAnimatorMove()
    {
        if (character == null) return;
        character.HandleAnimatorMove(animator.deltaPosition, animator.deltaRotation);
    }
}
