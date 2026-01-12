using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour,IInitializedAble
{
    public static readonly float reachCornerDistance = 0.75f;

    // ===== Public API (NavMeshAgent-like) =====
    public Vector3 destination { get; private set; }
    public Vector3 steeringTarget { get; private set; }

    public bool hasPath => _path != null && _path.corners.Length > 0;

    // ===== Internal =====
    private Transform _owner => transform;
    private NavMeshPath _path;
    [SerializeField] private int _cornerIndex;
    [SerializeField] private int _cornerLenght;

    [SerializeField] bool isHasPath;

    // ===== Unity =====
    public void Initialized()
    {
        _path = new NavMeshPath();
    }
    private void FixedUpdate()
    {

        isHasPath = hasPath;

        if (!hasPath)
            return;

        _cornerLenght = _path.corners.Length;

        UpdateSteeringTarget();
    }

   [SerializeField] private bool isCalculatePath;
    // ===== Public Methods =====
    public void SetDestination(Vector3 target)
    {

        //Debug.Log("SetDestination = " + target);
        //Debug.DrawLine(this.transform.position, target, Color.yellow);


        // 1️⃣ Clamp destination onto NavMesh
        NavMeshHit hit;
        bool found = ç(
            target,
            out hit,
            2.0f,                 // search radius (tune this)
            NavMesh.AllAreas
        );

        //Debug.Log("NavMesh.SamplePosition found? = " + found);

        if (!found)
        {
            pathPending = false;
            return;
        }

        destination = hit.position;

        if(isCalculatePath)
            return;

        // 2️⃣ Calculate path
        isCalculatePath = true;
        bool success = NavMesh.CalculatePath(
            _owner.position,
            destination,
            NavMesh.AllAreas,
            _path
        );
        isCalculatePath = false;

        pathPending = false;

        //Debug.Log("NavMesh.CalculatePath success? = "+success);

        if (!success || _path.status == NavMeshPathStatus.PathInvalid)
        {
            return;
        }

        // 3️⃣ Initialize steering
        if (_path.corners.Length > 0)
            steeringTarget = _path.corners[0];
        

        return;
    }

    public void ResetPath()
    {
        _path.ClearCorners();
        _cornerIndex = 0;
        desiredDirection = Vector3.zero;
        steeringTarget = Vector3.zero;
    }

    // ===== Core Logic =====
    private void UpdateSteeringTarget()
    {
        if (_cornerIndex >= _path.corners.Length)
        {
            desiredDirection = Vector3.zero;
            return;
        }

        steeringTarget = _path.corners[_cornerIndex];

        Vector3 toCorner = steeringTarget - _owner.position;
        toCorner.y = 0f;

        float distance = toCorner.magnitude;

        Debug.Log("steeringTarge = " + steeringTarget + "_owner.position = "+ _owner.position);
        Debug.Log("steerTarget distance = " + distance);

        // Reached this corner → advance
        if (distance <= reachCornerDistance)
        {
            _cornerIndex++;

            if (_cornerIndex < _path.corners.Length)
                steeringTarget = _path.corners[_cornerIndex];
            else
                desiredDirection = Vector3.zero;

            return;
        }

        desiredDirection = toCorner.normalized;
    }

    // ===== Helper =====
    private float CalculateRemainingDistance()
    {
        if (!hasPath || _cornerIndex >= _path.corners.Length)
            return 0f;

        float distance = Vector3.Distance(
            _owner.position,
            _path.corners[_cornerIndex]
        );

        for (int i = _cornerIndex; i < _path.corners.Length - 1; i++)
        {
            distance += Vector3.Distance(
                _path.corners[i],
                _path.corners[i + 1]
            );
        }

        return distance;
    }


    private void OnDrawGizmos()
    {
        if (Application.isPlaying == false)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position,steeringTarget);
        Gizmos.DrawSphere(steeringTarget, 0.5f);

        Vector3[] drawPositions = _path.corners;

        if (drawPositions == null || drawPositions.Length < 2)
            return;

        Gizmos.color = Color.cyan;

        for (int i = 0; i < drawPositions.Length - 1; i++)
        {
            Gizmos.DrawLine(drawPositions[i], drawPositions[i + 1]);
            Gizmos.DrawSphere(drawPositions[i], 0.08f);
        }

        // Draw last point
        Gizmos.DrawSphere(drawPositions[drawPositions.Length - 1], 0.08f);
    }

}
