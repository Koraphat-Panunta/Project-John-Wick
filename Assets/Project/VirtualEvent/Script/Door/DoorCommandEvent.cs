using UnityEngine;

public class DoorCommandEvent : VirtualEventNode
{
    public enum CommandEvent
    {
        Open,
        Close,
        Interact
    }

    public CommandEvent command;
    public override void Execute()
    {
        if (doorActor != null)
        {
            switch (command)
            {
                case CommandEvent.Open: this.doorActor.Open();
                    break;
                case CommandEvent.Close: this.doorActor.Close();
                    break;
                case CommandEvent.Interact: this.doorActor.DoInteract();
                    break;
            }
        }
        base.Execute();
    }

    protected override void OnDrawGizmos()
    {
        if(isEnableGizmos
            && doorActor != null)
        {
            onDrawGizmosTriggerEvent.DrawSphere(transform.position, doorActor._transform.position,color);
        }
        base.OnDrawGizmos();
    }

    [SerializeField] protected DoorActor doorActor;
}
