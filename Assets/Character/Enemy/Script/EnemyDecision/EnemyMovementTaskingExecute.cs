using UnityEngine;

public class EnemyMoveToPos : ITaskingExecute
{
    private readonly Vector3 _pos;
    private readonly bool _rotateToward;
    private readonly Transform _transform;
    private readonly EnemyCommandAPI _api;
    private readonly float _reachDistance;

    public EnemyMoveToPos(Transform transform, Vector3 pos, bool rotateToward,
        EnemyCommandAPI api, float reachDistance = 1f)
    {
        _pos = pos;
        _rotateToward = rotateToward;
        _transform = transform;
        _api = api;
        _reachDistance = reachDistance;
    }

    public void Update()
    {
        if (_rotateToward)
            _api.MoveToPositionRotateToward(_pos, 1, 1, _reachDistance);
        else
            _api.MoveToPosition(_pos, 1, _reachDistance);
    }

    public void FixedUpdate() { }

    public bool IsComplete() =>
        Vector3.Distance(_transform.position, _pos) <= _reachDistance;
}

public class EnemyRotateToPos : ITaskingExecute
{
    private readonly Vector3 _targetPos;
    private readonly float _rotateSpeed;
    private readonly Transform _transform;
    private readonly EnemyCommandAPI _api;

    public EnemyRotateToPos(Transform transform, Vector3 targetPos,
        float rotateSpeed, EnemyCommandAPI api)
    {
        _transform = transform;
        _targetPos = targetPos;
        _rotateSpeed = rotateSpeed;
        _api = api;
    }

    public void Update() => _api.RotateToPosition(_targetPos, _rotateSpeed);
    public void FixedUpdate() { }

    public bool IsComplete()
    {
        Vector3 dir = (_targetPos - _transform.position).normalized;
        return Vector3.Dot(_transform.forward, dir) > 0.95f;
    }
}
