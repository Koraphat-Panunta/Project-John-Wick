using UnityEngine;

public abstract class DropAbleObjectClient : MonoBehaviour,IHPDropAble,IAmmoDropAble
{
    Transform IDropAble.transform { get => transform; }
    [SerializeField] protected HpGetAbleObject HpGetAbleObject ;
    [SerializeField] protected AmmoGetAbleObject AmmoGetAbleObject ;

    [Range(0, 100)]
    [SerializeField] protected float SpawnForce;
    public void DropObject(HpGetAbleObject hpGetAbleObject)
    {
        Debug.Log("DropObj");
        HpGetAbleObject hpObj = GameObject.Instantiate(hpGetAbleObject,transform.position,transform.rotation) as HpGetAbleObject;
        Vector3 spawnForce = (Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up).eulerAngles).normalized;
        hpObj.rb.AddForce(spawnForce*this.SpawnForce, ForceMode.Impulse);
    }

    public void DropObject(AmmoGetAbleObject ammoGetAbleObject)
    {
        AmmoGetAbleObject hpObj = GameObject.Instantiate(ammoGetAbleObject, transform.position, transform.rotation) as AmmoGetAbleObject;
        Vector3 spawnForce = (Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up).eulerAngles).normalized;
        hpObj.rb.AddForce(spawnForce * this.SpawnForce, ForceMode.Impulse);
    }
}
