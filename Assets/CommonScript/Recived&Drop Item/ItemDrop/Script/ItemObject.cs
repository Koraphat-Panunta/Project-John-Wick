using UnityEngine;
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public abstract class ItemObject<T> : MonoBehaviour where T : IRecivedAble
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected T clent;
    protected bool isBeenPull;
    protected Rigidbody rb;
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
        this.rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isBeenPull)
        {
            Debug.Log("Item Distance 2 = " + Vector3.Distance(clent.transform.position, transform.position));

            rb.useGravity = false;
            colliding.enabled = false;
            Vector3 direction = (clent.transform.position - transform.position).normalized;

            Debug.Log("Item Distance 3 = " + Vector3.Distance(clent.transform.position, transform.position));

            rb.AddForce(direction * pullStrenght,ForceMode.VelocityChange);
            if (rb.linearVelocity.magnitude > maxSpeedPull)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeedPull;
            }

            Debug.Log("Client Pos = " + clent.transform.position);

            Debug.Log("Item Distance 4 = " + Vector3.Distance(clent.transform.position, transform.position));

            if (Vector3.Distance(clent.transform.position,transform.position) < 0.45f)
            {
                SetVisitorClient(clent);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Obj Enter");
        if (other.gameObject.TryGetComponent<T>(out T recieved))
        {
            Debug.Log("Obj is "+recieved);

            //Ray ray = new Ray(transform.position, (recieved.transform.position - transform.position).normalized);

            //if(Physics.Raycast(ray,out RaycastHit hitInfo,detectRecievedRagne,0) == false)
            //    return;

            //if(hitInfo.collider.gameObject != other.gameObject)
            //    return ;

            Debug.Log("Obj is BeenPull");

            this.clent = recieved;
            this.isBeenPull = true;

            Debug.Log("Item Distance 1 = " + Vector3.Distance(clent.transform.position, transform.position));
        }
    }
    protected abstract void SetVisitorClient(T client);
}
