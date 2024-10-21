using Cinemachine;
using Cinemachine.Editor;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class CameraHandShake : ICameraAction
{
    private CameraController camera;
    public CinemachineImpulseSource impulseSource;
    public CameraHandShake(CameraController camera) 
    {
        this.camera = camera;
        this.impulseSource = this.camera.impulseSource;
    }
    public void Performed()
    {
        impulseSource.GenerateImpulse();
    }
}
