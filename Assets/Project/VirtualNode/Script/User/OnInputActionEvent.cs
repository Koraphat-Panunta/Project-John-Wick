using UnityEngine;
using UnityEngine.InputSystem;

public class OnInputActionEvent : VirtualEventNode,IObserverActor
{
    [SerializeField] protected UserInputActor userActor;
    [SerializeField] protected UserInputActor.InpuPhase inpuPhase;
    [SerializeField] protected string inputActionName;


    private void Awake()
    {
        this.userActor.AddActorObserver(this);
    }
    public void OnNotifyActor<T>(Actor actor, T var)
    {
        if(actor is UserInputActor
            && var is InputAction.CallbackContext ctx
            && ctx.action.name == this.inputActionName
           )
        {
            if (inpuPhase == UserInputActor.InpuPhase.performed
                && ctx.performed)
                Execute();

            if(inpuPhase == UserInputActor.InpuPhase.canceled
                && ctx.canceled)
                Execute();
        }
    }

    protected override void OnDrawGizmos()
    {
        if(isEnableGizmos
            && userActor != null)
        {
            onDrawGizmosTriggerEvent.DrawLine(this.userActor.transform.position,this.transform.position,color);
        }
        base.OnDrawGizmos();
    }
}
