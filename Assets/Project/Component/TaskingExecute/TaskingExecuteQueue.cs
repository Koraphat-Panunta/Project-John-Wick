using System.Collections.Generic;

public class TaskingExecuteQueue
{
    private readonly Queue<ITaskingExecute> _tasks = new Queue<ITaskingExecute>();

    public int Count => _tasks.Count;
    public bool IsEmpty => _tasks.Count == 0;

    public void Enqueue(ITaskingExecute task) => _tasks.Enqueue(task);

    public void Update()
    {
        if (_tasks.Count == 0) return;
        if (_tasks.Peek().IsComplete())
            _tasks.Dequeue();
        if (_tasks.Count > 0)
            _tasks.Peek().Update();
    }

    public void FixedUpdate()
    {
        if (_tasks.Count == 0) return;
        _tasks.Peek().FixedUpdate();
    }
}
