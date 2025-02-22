using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public abstract class ItemObject<T> : MonoBehaviour where T : IRecivedAble
{
    // Start is called once before the first execution of UpdateNode after the MonoBehaviour is created
    protected T clent;
    [SerializeField] protected bool isBeenPull;

    [SerializeField]protected float elapseTimePullAble;

    public Rigidbody rb { get; protected set; }
    [SerializeField] protected Collider colliding;
    

    [Range(0,10)]
    [SerializeField] protected float detectRecievedRagne;

    [Range(0, 100)]
    [SerializeField] protected float pullStrenght;

    [Range(0, 100)]
    [SerializeField] protected float maxSpeedPull;
    void Start()
    {
        isBeenPull = false;

    }
    private void OnDisable()
    {
        elapseTimePullAble = 0;
    }
    private void OnEnable()
    {
        this.rb = GetComponent<Rigidbody>();
        elapseTimePullAble = 0;
    }
    // UpdateNode is called once per frame
    protected virtual void Update()
    {
        if(elapseTimePullAble < 2)
        elapseTimePullAble += Time.deltaTime;

        if (isBeenPull == false)
        {
            Collider[] col = Physics.OverlapSphere(transform.position, detectRecievedRagne);
            foreach (Collider n in col)
            {
                if (n.TryGetComponent<T>(out T r))
                {
                    if (r.PreCondition(r))
                    {
                        this.clent = r;
                        isBeenPull = true;
                    }
                }
            }
        }

        if (isBeenPull && elapseTimePullAble >= 2)
        {
            rb.useGravity = false;
            colliding.enabled = false;
            Vector3 direction = (clent.transform.position - transform.position).normalized;

     

            rb.AddForce(direction * pullStrenght,ForceMode.VelocityChange);
            if (rb.linearVelocity.magnitude > maxSpeedPull)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeedPull;
            }

          

            if (Vector3.Distance(clent.transform.position,transform.position) < 0.45f)
            {
                SetVisitorClient(clent);
                Destroy(gameObject);
            }
        }
    }
    protected abstract void SetVisitorClient(T client);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectRecievedRagne);
    }
}
