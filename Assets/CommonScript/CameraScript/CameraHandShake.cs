using Cinemachine;

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
