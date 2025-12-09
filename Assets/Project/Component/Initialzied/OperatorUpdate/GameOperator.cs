using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-400)]
public class GameOperator : MonoBehaviour
{
    static public GameOperator instance;

    public Dictionary<int, List<IUpdateAble>> updateOperatorObject;
    public Dictionary<int, List<IFixedUpdateAble>> fixedUpdateOperatorObject;
    public Dictionary<int, List<ILateUpdateAble>> latedUpdateOperatorObject;


    private void Awake()
    {
        instance = this;

        this.updateOperatorObject = new Dictionary<int, List<IUpdateAble>>();
        this.fixedUpdateOperatorObject = new Dictionary<int, List<IFixedUpdateAble>>();
        this.latedUpdateOperatorObject = new Dictionary<int, List<ILateUpdateAble>>();

    }
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if(this.fixedUpdateOperatorObject.Count <= 0)
            return;

        for (int i = 0; i < this.fixedUpdateOperatorObject.Count; i++) 
        {
            if (this.fixedUpdateOperatorObject[i].Count <= 0)
                continue;
            for (int j = 0; j < this.fixedUpdateOperatorObject[i].Count; j++) 
            {
                IFixedUpdateAble fixedUpdateAble = this.fixedUpdateOperatorObject[i][j];
                if (fixedUpdateAble.isActive == false)
                    continue;
                fixedUpdateAble.FixedUpdateOperator();
            }
        }
    }
    // Update is called once per frame
    private void Update()
    {
        if (this.updateOperatorObject.Count <= 0)
            return;

        for (int i = 0; i < this.updateOperatorObject.Count; i++)
        {
            if (this.updateOperatorObject[i].Count <= 0)
                continue;
            for (int j = 0; j < this.updateOperatorObject[i].Count; j++)
            {
                IUpdateAble updateAble = this.updateOperatorObject[i][j];
                if(updateAble.isActive == false)
                    continue;
                updateAble.UpdateOperator();
            }
        }
    }
    
   
    private void LateUpdate()
    {
        if (this.latedUpdateOperatorObject.Count <= 0)
            return;

        for (int i = 0; i < this.latedUpdateOperatorObject.Count; i++)
        {
            if (this.latedUpdateOperatorObject[i].Count <= 0)
                continue;
            for (int j = 0; j < this.latedUpdateOperatorObject[i].Count; j++)
            {
                ILateUpdateAble lateUpdateAble = this.latedUpdateOperatorObject[i][j];
                if (lateUpdateAble.isActive == false)
                    continue;
                lateUpdateAble.LateUpdateOperator();
            }
        }
    }

    public void AddUpdateOperator(Component component)
    {
        if(component.TryGetComponent<IFixedUpdateAble>(out IFixedUpdateAble fixedUpdateAble))
        {
            if (this.fixedUpdateOperatorObject.TryGetValue(fixedUpdateAble._fixedUpdateOrder, out List<IFixedUpdateAble> fixedUpdates) == false)
                this.fixedUpdateOperatorObject.Add(fixedUpdateAble._fixedUpdateOrder, new List<IFixedUpdateAble>());

            this.fixedUpdateOperatorObject[fixedUpdateAble._fixedUpdateOrder].Add(fixedUpdateAble);
        }

        if (component.TryGetComponent<IUpdateAble>(out IUpdateAble updateAble))
        {
            if (this.updateOperatorObject.TryGetValue(updateAble._updatedOrder, out List<IUpdateAble> updateAbles) == false)
                this.updateOperatorObject.Add(updateAble._updatedOrder, new List<IUpdateAble>());

            this.updateOperatorObject[updateAble._updatedOrder].Add(updateAble);
        }

        if (component.TryGetComponent<ILateUpdateAble>(out ILateUpdateAble lateUpdateAble))
        {
            if (this.latedUpdateOperatorObject.TryGetValue(lateUpdateAble._latedUpdateOrder, out List<ILateUpdateAble> lateUpdateAbles) == false)
                this.latedUpdateOperatorObject.Add(lateUpdateAble._latedUpdateOrder, new List<ILateUpdateAble>());

            this.latedUpdateOperatorObject[lateUpdateAble._latedUpdateOrder].Add(lateUpdateAble);
        }
    }

    public void RemovedUpdateOperator(Component component)
    {
        if (component.TryGetComponent<IFixedUpdateAble>(out IFixedUpdateAble fixedUpdateAble))
        {
            if (this.fixedUpdateOperatorObject.TryGetValue(fixedUpdateAble._fixedUpdateOrder, out List<IFixedUpdateAble> fixedUpdates) == false)
                this.fixedUpdateOperatorObject.Add(fixedUpdateAble._fixedUpdateOrder, new List<IFixedUpdateAble>());

            this.fixedUpdateOperatorObject[fixedUpdateAble._fixedUpdateOrder].Remove(fixedUpdateAble);
        }

        if (component.TryGetComponent<IUpdateAble>(out IUpdateAble updateAble))
        {
            if (this.updateOperatorObject.TryGetValue(updateAble._updatedOrder, out List<IUpdateAble> updateAbles) == false)
                this.updateOperatorObject.Add(updateAble._updatedOrder, new List<IUpdateAble>());

            this.updateOperatorObject[updateAble._updatedOrder].Remove(updateAble);
        }

        if (component.TryGetComponent<ILateUpdateAble>(out ILateUpdateAble lateUpdateAble))
        {
            if (this.latedUpdateOperatorObject.TryGetValue(lateUpdateAble._latedUpdateOrder, out List<ILateUpdateAble> lateUpdateAbles) == false)
                this.latedUpdateOperatorObject.Add(lateUpdateAble._latedUpdateOrder, new List<ILateUpdateAble>());

            this.latedUpdateOperatorObject[lateUpdateAble._latedUpdateOrder].Remove(lateUpdateAble);
        }
    }

}
