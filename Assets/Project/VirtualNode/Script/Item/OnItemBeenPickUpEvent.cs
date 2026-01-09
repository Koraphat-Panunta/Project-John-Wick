using UnityEngine;

public class OnItemBeenPickUpEvent : VirtualEventNode, IObserverItem
{
    [SerializeField] ItemObject ItemObject;

    private void Awake()
    {
        ItemObject.AddObserver(this);
    }
   

   

    protected override void OnDrawGizmos()
    {
        if (isEnableGizmos
            && ItemObject != null)
        {
            Gizmos.color = color;
            Gizmos.DrawLine(this.transform.position, this.ItemObject.transform.position);
        }
        base.OnDrawGizmos();
    }

    public void OnNotifyObserver(ItemObject itemObject, ItemObject.ItemNotifyMassage itemNotifyMassage)
    {
        if(itemNotifyMassage == ItemObject.ItemNotifyMassage.PickedUp)
        {
            this.Execute();
            itemObject.RemovedObserver(this);
        }
    }
}
