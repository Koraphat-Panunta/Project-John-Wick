using System.Collections.Generic;
using UnityEngine;

public class BlackBoard 
{
    private Dictionary<string, object> _data = new Dictionary<string, object>();

    public virtual T Get<T>(string key)
    {
        return _data.TryGetValue(key, out var value) ? (T)value : default;
    }

    public virtual void Set<T>(string key, T value)
    {
        _data[key] = value;
    }
}
