using System;
using UnityEngine;

public class TaskingExecute : ITaskingExecute
{
    private Action update;
    private Action fixUpdate;
    private Func<bool> isComplete;
    public TaskingExecute(Action update, Action fixUpdate, Func<bool> isComplete)
    {
        this.update = update;
        this.fixUpdate = fixUpdate;
        this.isComplete = isComplete;
    }
    public TaskingExecute(Action update, Func<bool> isComplete) : this(update, null, isComplete)
    {

    }
    public void FixedUpdate()
    {
        if (this.fixUpdate != null)
            this.fixUpdate.Invoke();
    }

    public bool IsComplete()
    {
        return isComplete.Invoke();
    }

    public void Update()
    {
        if (this.update != null)
            this.update.Invoke();
    }
}
