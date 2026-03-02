using UnityEngine;

public interface INodeNotifyBackAble
{
    public void OnNotifyBack<T>(INode node,T var);
}
