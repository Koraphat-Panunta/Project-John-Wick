using System;
using UnityEngine;

public class InteractAbleActor : Actor,I_Interactable
{


    [SerializeField] VirtualEventNode[] onNotifyUnInteracble;
    [SerializeField] VirtualEventNode[] onNotifyInteracble;
    public Collider _collider { get ; set ; }
    public Transform _transform { get => this.transform;set { } }
    [SerializeField] private bool isInteractAble;
    public bool isBeenInteractAble { get => this.isInteractAble ; set  => this.isInteractAble = value; }
    public Action<I_Interactable> onDoInteract { get; set; }

    public void DoInteract(I_Interacter i_Interacter)
    {
        if (this.isBeenInteractAble == false)
        {
            this.OnNotifyInteract(this.onNotifyUnInteracble);
            return;
        }

        this.OnNotifyInteract(this.onNotifyInteracble);
    }
    public void EnablieIntartactAble() => this.isBeenInteractAble = true;
    public void DisableInteractAlbe() => this.isBeenInteractAble = false;

    public virtual void OnNotifyInteract(VirtualEventNode[] onNotifyInteracble)
    {
        if(onNotifyInteracble == null
            || onNotifyInteracble.Length <= 0)
            return;

        for (int i = 0; i < onNotifyInteracble.Length; i++) 
        { onNotifyInteracble[i].Execute(); }
    }

    private OnDrawGizmosTriggerEvent _drawGizmos = new OnDrawGizmosTriggerEvent();

    private void OnValidate()
    {
        _drawGizmos.isDrawEnable = isEnableGizmos;
    }

    protected override void OnDrawGizmos()
    {
        if (isEnableGizmos)
        {
            if (onNotifyInteracble != null)
                for (int i = 0; i < onNotifyInteracble.Length; i++)
                    if (onNotifyInteracble[i] != null)
                        _drawGizmos.DrawSphere(transform.position, onNotifyInteracble[i].transform.position, Color.green);

            if (onNotifyUnInteracble != null)
                for (int i = 0; i < onNotifyUnInteracble.Length; i++)
                    if (onNotifyUnInteracble[i] != null)
                        _drawGizmos.DrawSphere(transform.position, onNotifyUnInteracble[i].transform.position, Color.red);
        }

        base.OnDrawGizmos();
    }

   
}
