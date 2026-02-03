using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class AIAgent : MonoBehaviour,IInitializedAble
{
    public static readonly float reachCornerDistance = 0.5f;

    // ===== Public API (NavMeshAgent-like) =====
    [SerializeField] protected Vector3 targetDestination;
    [SerializeField] public Vector3 destination; 
    public Vector3 steeringTarget { 
        get
        {
            if(cornerPostion.Count <= 0)
                return this.transform.position;

            return cornerPostion.Peek();
        } private set { } }

    public bool hasPath => cornerPostion != null && cornerPostion.Count > 0;

    [SerializeField] protected Queue<Vector3> cornerPostion;

    // ===== Internal =====
    private Transform _owner => transform;
    private NavMeshPath _path;

    [SerializeField] private bool isPerforming;

    // ===== Unity =====
    public void Initialized()
    {
        _path = new NavMeshPath();
        cornerPostion = new Queue<Vector3>();
    }

  
    private void FixedUpdate()
    {
        UpdatePathDestination();
        UpdateSteeringTarget();
    }

   
    [SerializeField] private bool isCalculatePath;
    // ===== Public Methods =====
    
    public void SetDestination(Vector3 target)
    {
        isPerforming = true;
        targetDestination = target;

    }
    public void ResetPath()
    {
        _path.ClearCorners();
        cornerPostion.Clear();
        steeringTarget = Vector3.zero;
        this.isPerforming = false;
    }

    // ===== Core Logic =====
    private void UpdateSteeringTarget()
    {
        if(hasPath == false)
            return;

        Vector3 targetPos = new Vector3(this.steeringTarget.x, this.transform.position.y, this.steeringTarget.z);

        if(Vector3.Distance(this.transform.position, targetPos) <= reachCornerDistance)
        {
            cornerPostion.Dequeue();

            if(cornerPostion.Count <= 0)
                isPerforming |= false;
        }
    }

    [SerializeField] private float timer;
    private float bufferTime = 1;
    [SerializeField] private bool isCalulatePath;

    private void UpdatePathDestination()
    {
        this.timer += Time.fixedDeltaTime;
        if(this.timer < bufferTime)
            return;

        this.timer = 0;

        if(this.isPerforming == false)
            return;

        //Debug.Log("UpdatePathDestination");


        //Find destination
        bool foundDestination = NavMesh.SamplePosition(
            this.targetDestination
            , out NavMeshHit hit
            , 10
            , NavMesh.AllAreas);

        if (foundDestination == false)
            Debug.LogError("Not found destination");

        this.destination = hit.position;


        //Find path
        if (this.isCalulatePath)
            return;

        this.isCalulatePath = true;
        bool foundPath = NavMesh.CalculatePath(
            this.transform.position
            , this.destination
            , NavMesh.AllAreas
            , _path);

        this.isCalulatePath = false;
        if (foundPath == false)
            Debug.LogError("Not found path");

        //Populate path.corner to cornerPosition
        if(_path.corners.Length <= 0)
            return;

        cornerPostion.Clear();
        for(int i = 0;i < _path.corners.Length; i++)
        {
            cornerPostion.Enqueue(_path.corners[i]);
        }

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
