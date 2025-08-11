using UnityEngine;

public abstract class DropAbleObjectClient : MonoBehaviour,IHPDropAble,IAmmoDropAble
{
    Transform IDropAble.transform { get => transform; }


    [Range(0, 100)]
    [SerializeField] protected float SpawnForceUp;

    [Range(0, 100)]
    [SerializeField] protected float SpawnForceSide;
    public void DropObject(HpGetAbleObject hpGetAbleObject)
    {
        HpGetAbleObject hpObj = GameObject.Instantiate(hpGetAbleObject,transform.position,transform.rotation) as HpGetAbleObject;

        Vector3 randomSideForce = Quaternion.Euler(0, Random.Range(0, 360), 0) * Vector3.right * SpawnForceSide; // Random lateral direction

        // Upward force
        Vector3 upwardForce = Vector3.up * SpawnForceUp;

        // Final force (combining upward + random sideways)
        Vector3 spawnForce = (randomSideForce + upwardForce);

        hpObj.rb.AddForce(spawnForce, ForceMode.Impulse);
    }

    public void DropObject(AmmoGetAbleObject ammoGetAbleObject)
    {
        AmmoGetAbleObject hpObj = GameObject.Instantiate(ammoGetAbleObject, transform.position, transform.rotation) as AmmoGetAbleObject;
        // Random sideways force (360° around Y-axis)
        Vector3 randomSideForce = Quaternion.Euler(0, Random.Range(0, 360), 0) * Vector3.right * SpawnForceSide; // Random lateral direction

        // Upward force
        Vector3 upwardForce = Vector3.up * SpawnForceUp;

        // Final force (combining upward + random sideways)
        Vector3 spawnForce = (randomSideForce + upwardForce);

        hpObj.rb.AddForce(spawnForce, ForceMode.Impulse);
    }
}
