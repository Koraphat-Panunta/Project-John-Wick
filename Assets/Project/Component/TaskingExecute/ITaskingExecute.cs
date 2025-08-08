using UnityEngine;

public interface ITaskingExecute 
{
    public void Update();
    public void FixedUpdate();
    public bool IsComplete();
}
