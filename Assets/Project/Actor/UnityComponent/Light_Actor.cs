using UnityEngine;

public class Light_Actor : Actor
{
    [SerializeField] public Light lightComponent;

    public void Open()
    {
        if(this.lightComponent != null)
            lightComponent.gameObject.SetActive(true);
    }

    public void Close()
    {
        if(this.lightComponent != null)
            lightComponent.gameObject.SetActive(false);
    }

    public void DoInteract()
    {
        if(this.lightComponent != null)
        {
            if(this.lightComponent.gameObject.activeInHierarchy == true)
                Close();
            else
                Open();
        }
    }

    protected override void OnDrawGizmos()
    {
        if (lightComponent != null && base.isEnableGizmos)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(lightComponent.transform.position, transform.position);
        }
        base.OnDrawGizmos();
    }
}
