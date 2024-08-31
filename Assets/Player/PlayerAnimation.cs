using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator animator;
    public enum parameterName
    {
        ForBack_Ward,
        Side_LR,
        IsSprinting,
        Reloading,
        TacticalReload
    }
    public Dictionary<parameterName, string> animationParameter = new Dictionary<parameterName, string>();
    void Start()
    {
        animationParameter.Add(parameterName.ForBack_Ward, "ForBack_Ward");
        animationParameter.Add(parameterName.Side_LR, "Side_LR");
        animationParameter.Add(parameterName.IsSprinting, "IsSprinting");
        animationParameter.Add(parameterName.Reloading, "Reloading");
        animationParameter.Add(parameterName.TacticalReload, "TacticalReload");
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void AnimateMove(PlayerMovement playerMovement)
    {
        float var = (playerMovement.curVelocity.z * 2) / playerMovement.move_MaxSpeed;
        animator.SetFloat(animationParameter[parameterName.ForBack_Ward], -1+var);
        var = (playerMovement.curVelocity.x * 2) / playerMovement.move_MaxSpeed;
        animator.SetFloat(animationParameter[parameterName.Side_LR], -1+var);
    }
}
