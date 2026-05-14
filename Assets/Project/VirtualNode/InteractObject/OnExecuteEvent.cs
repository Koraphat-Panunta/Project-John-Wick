using UnityEngine;

public class OnExecuteEvent : VirtualEventNode
{
    public bool isExecuteOnce;
    protected bool isAlreadyExecute;
    public override void Execute()
    {
        if (this.isExecuteOnce && this.isAlreadyExecute)
            return;

        base.Execute();
        this.isAlreadyExecute = true;
    }
}
