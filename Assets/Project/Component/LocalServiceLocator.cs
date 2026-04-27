using System;
using System.Collections.Generic;
using UnityEngine;

public class LocalServiceLocator : MonoBehaviour
{
    // The dictionary is no longer static. Every instance of this script gets its own unique dictionary.
    private readonly Dictionary<Type, object> services = new Dictionary<Type, object>();
    /// <summary>
    /// Registers a service to this specific local locator.
    /// </summary>
    public void Register<T>(T service)
    {
        Type type = typeof(T);
        if (!services.ContainsKey(type))
        {
            services.Add(type, service);
        }
        else
        {
            // Warning instead of error, as you might intentionally overwrite local services in some setups
            Debug.LogWarning($"[Local Locator - {gameObject.name}] Service of type {type} is already registered. Overwriting.");
            services[type] = service;
        }
    }

    /// <summary>
    /// Removes a service from this specific local locator.
    /// </summary>
    public void Unregister<T>()
    {
        Type type = typeof(T);
        if (services.ContainsKey(type))
        {
            services.Remove(type);
        }
    }

    /// <summary>
    /// Retrieves a service from this specific local locator.
    /// </summary>
    public T Get<T>()
    {
        Type type = typeof(T);
        if (services.TryGetValue(type, out object service))
        {
            return (T)service;
        }

        Debug.LogError($"[Local Locator - {gameObject.name}] Service of type {type} not found! Did you register it?");
        return default;
    }

    /// <summary>
    /// Safe getter that doesn't throw an error if the service is missing.
    /// </summary>
    public bool TryGet<T>(out T service)
    {
        Type type = typeof(T);
        if (services.TryGetValue(type, out object foundService))
        {
            service = (T)foundService;
            return true;
        }

        service = default;
        return false;
    }
}
