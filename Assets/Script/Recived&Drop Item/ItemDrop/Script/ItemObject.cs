using UnityEngine;
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public abstract class ItemObject<T> : MonoBehaviour where T : IRecivedAble
{
    // Start is called once before the first execution of UpdateNode after the MonoBehaviour is created
    protected T clent;
    protected bool isBeenPull;

    protected float elapseTimePullAble;

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
        //this.rb = GetComponent<Rigidbody>();
        elapseTimePullAble = 0;
    }
    private void OnEnable()
    {
        this.rb = GetComponent<Rigidbody>();
        elapseTimePullAble = 0;
    }
    // UpdateNode is called once per frame
    void Update()
    {
        elapseTimePullAble += Time.deltaTime;

        if (isBeenPull&&elapseTimePullAble >= 2)
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<T>(out T recieved))
        {

            //Ray ray = new Ray(transform.position, (recieved.transform.position - transform.position).normalized);

            //if(Physics.Raycast(ray,out RaycastHit hitInfo,detectRecievedRagne,0) == false)
            //    return;

            //if(hitInfo.collider.gameObject != other.gameObject)
            //    return ;

            this.clent = recieved;
            this.isBeenPull = true;

        }
    }
    protected abstract void SetVisitorClient(T client);
}
