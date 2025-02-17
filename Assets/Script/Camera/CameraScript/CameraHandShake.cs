using Cinemachine;

public class CameraHandShake : ICameraAction
{
    private CameraController camera;
    public CinemachineImpulseSource impulseSource => camera.impulseSource;
    public CameraHandShake(CameraController camera) 
    {
        this.camera = camera;
    }
    public void Performed()
    {
        impulseSource.GenerateImpulse();
    }
}
