using UnityEngine;

public class CameraYawRotator
{
    private readonly ThirdPersonCinemachineCamera _camera;
    private readonly float _speed;

    public CameraYawRotator(ThirdPersonCinemachineCamera camera, float speed)
    {
        _camera = camera;
        _speed = speed;
    }

    public void RotateTowards(Vector3 direction)
    {
        Vector3 flat = new Vector3(direction.x, 0f, direction.z);
        if (flat == Vector3.zero) return;

        float targetYaw = Quaternion.LookRotation(flat.normalized, Vector3.up).eulerAngles.y;
        _camera.SetYaw(Mathf.MoveTowardsAngle(_camera.yaw, targetYaw, _speed * Time.deltaTime));
    }
}
