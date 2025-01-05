using UnityEngine;

public class PatrolPoint : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] public Transform patrolTrans;
    [Range(0,10)]
    public float waitTime;
    private void Awake()
    {
        patrolTrans = transform;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(transform.position, 0.17f);
    }
}
