using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnvironmentAware
{
    public void OnAware(GameObject sourceFrom,EnvironmentType environmentType);
}
