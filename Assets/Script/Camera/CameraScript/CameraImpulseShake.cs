using Unity.Cinemachine;
using UnityEngine;

public class CameraImpulseShake : ICameraAction
{
    private CameraController camera;
    public CinemachineImpulseSource impulseSource => camera.impulseSource;
    public CameraImpulseShake(CameraController camera) 
    {
        this.camera = camera;
    }
    public void Performed()
    {
        impulseSource.GenerateImpulse();
    }
    public void Performed(float intensity)
    {
        impulseSource.GenerateImpulseWithForce(intensity);
    }
    public void Performed(Vector3 force)
    {
        impulseSource.GenerateImpulse(force);
    }
}
