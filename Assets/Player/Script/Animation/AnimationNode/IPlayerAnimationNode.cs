using UnityEngine;

public interface IPlayerAnimationNode 
{
    public Animator animator { get; set; }
    public PlayerAnimationManager playerAnimationManager { get; set; }
    public PlayerAnimationNodeLeaf curAnimationNodeLeaf { get; set; }
    public PlayerAnimationNodeSelector startSelectorAnimationNode { get; set; }
    public void InitailizedPlayerAnimationNode();
    public void UpdateNode();
    public void FixedUpdateNode();
}
