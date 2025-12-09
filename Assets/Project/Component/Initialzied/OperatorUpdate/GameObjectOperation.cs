using System.Collections.Generic;
using UnityEngine;

public class GameObjectOperation : MonoBehaviour, IUpdateAble, IFixedUpdateAble, ILateUpdateAble
{

    [SerializeField] private int updateOrder;
    public int _updatedOrder => this.updateOrder;

    [SerializeField] private int fixedUpdateOrder;
    public int _fixedUpdateOrder => this.fixedUpdateOrder;

    [SerializeField] private int lateUpdateOrder;
    public int _latedUpdateOrder => this.lateUpdateOrder;

    public bool isActive => gameObject.activeInHierarchy;

    [SerializeField] protected Component[] updateOperates;

    protected IFixedUpdateAble[] fixedUpdateAbles;
    protected IUpdateAble[] updateAbles;
    protected ILateUpdateAble[] lateUpdateAbles;

    public void FixedUpdateOperator()
    {
        if(this.fixedUpdateAbles == null || this.fixedUpdateAbles.Length <= 0)
            return;

        for (int i = 0; i < this.fixedUpdateAbles.Length; i++) 
        {
            this.fixedUpdateAbles[i].FixedUpdateOperator();
        }
        
    }

    public void LateUpdateOperator()
    {
        if(this.lateUpdateAbles == null || this.lateUpdateAbles.Length <= 0)
            return;

        for(int i = 0; i < this.lateUpdateAbles.Length; i++)
        {
            this.lateUpdateAbles[i].LateUpdateOperator();
        }
    }

    public void UpdateOperator()
    {
        if(this.updateAbles == null || this.updateAbles.Length <= 0)
            return;

        for(int i = 0; i < this.updateAbles.Length; i++)
        {
            this.updateAbles[i].UpdateOperator();
        }
    }

    private void OnValidate()
    {
        List<IFixedUpdateAble> list_FixedUpdate = new List<IFixedUpdateAble>();
        List<IUpdateAble> list_Update = new List<IUpdateAble>();
        List<ILateUpdateAble> list_LateUpdate = new List<ILateUpdateAble>();

        this.fixedUpdateAbles = null;
        this.updateAbles = null;
        this.lateUpdateAbles = null;

        if(this.updateOperates.Length <= 0)
            return;

        for (int i = 0; i < this.updateOperates.Length; i++) 
        {

            if(this.updateOperates[i] is IFixedUpdateAble fixedUpdate)
                list_FixedUpdate.Add(fixedUpdate);
            if(this.updateOperates[i] is IUpdateAble update)
                list_Update.Add(update);
            if (this.updateOperates[i] is ILateUpdateAble lateUpdate)
                list_LateUpdate.Add(lateUpdate);
            
        }

        this.fixedUpdateAbles = list_FixedUpdate.ToArray();
        this.updateAbles = list_Update.ToArray();
        this.lateUpdateAbles = list_LateUpdate.ToArray();
        
    }
}
