using UnityEngine;

public class Recoil : MonoBehaviour
{
    public float recoilAmount = 5f;
    public float recoilSpeed = 0.1f;

    private Vector3 originalRotation;
    private Vector3 currentRecoil;

    public bool TriggerRecoil = false;
    private Vector3 CurrentCameraTrans;

    void Start()
    {
        originalRotation = transform.localEulerAngles;
    }

    void Update()
    {
        transform.localEulerAngles += new Vector3(0, 100, 0);
        if (TriggerRecoil == true)
        {
            AddRecoil();
            TriggerRecoil = false ;
        }
        if (currentRecoil.magnitude > 0.1f)
        {
            currentRecoil = Vector3.Lerp(currentRecoil, Vector3.zero, Time.deltaTime * recoilSpeed);
        }
        transform.localEulerAngles = originalRotation + currentRecoil;

    }

    public void AddRecoil()
    {
        currentRecoil += new Vector3(-1,Random.Range(-0.5f,0.5f),0) * recoilAmount;
    }
}
