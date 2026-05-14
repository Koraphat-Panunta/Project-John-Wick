using UnityEngine;

public class DoorCommandEvent : VirtualEventNode,I_Interacter
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
        this.Interact();
        base.Execute();
    }

    protected override void OnDrawGizmos()
    {
        if(isEnableGizmos
            && doorActor != null)
        {
            Gizmos.color = color;
            Gizmos.DrawLine(transform.position, doorActor._transform.position);
        }
        base.OnDrawGizmos();
    }

    public void Interact()
    {
        if (doorActor != null)
        {
            switch (command)
            {
                case CommandEvent.Open:
                    this.doorActor.Open();
                    break;
                case CommandEvent.Close:
                    this.doorActor.Close();
                    break;
                case CommandEvent.Interact:
                    this.doorActor.DoInteract(this);
                    break;
            }
        }
    }

    [SerializeField] protected DoorActor doorActor;

    public I_Interactable currentInteractable { get => this.doorActor; set { } }
}
