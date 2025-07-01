using UnityEngine;

public interface IGunFuExecuteNodeLeaf : INodeLeaf,IGunFuNode
{
    public string _stateName { get; }
    protected bool _isExecuteAldready { get; set; }
}
