using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView
{
    private float Radiant;
    private float AngelInDegree;
    private GameObject objView;

    public FieldOfView(float radiant, float angelInDegree, GameObject objView)
    {
        Radiant = radiant;
        AngelInDegree = angelInDegree;
        this.objView = objView;
    }
    public GameObject FindSingleObjectInView(LayerMask targetMask)
    {
        Collider[] obj = Physics.OverlapSphere(objView.transform.position, this.Radiant, targetMask);
        GameObject returnObj = null;
        if (obj.Length > 0)
        {
            foreach (Collider tarObj in obj)
            {
                Vector3 Objdirection = tarObj.transform.position - objView.transform.position;
                Objdirection.Normalize();
                if (Vector3.Angle(objView.transform.forward, Objdirection) < AngelInDegree / 2)
                {
                    Debug.Log("Finded Object");
                    //if (Physics.Raycast(objView.transform.position, tarObj.transform.position, out RaycastHit hit, 1000))
                    //{
                    //    if (hit.collider.gameObject == tarObj.gameObject)
                    //    {
                    //        returnObj = tarObj.gameObject;
                    //    }
                    //}
                    returnObj = tarObj.gameObject;
                }
            }
        }
        return returnObj;
    }
    public List<GameObject> FindMutipleObjectInView(LayerMask targetMask)
    {
        Collider[] obj = Physics.OverlapSphere(objView.transform.position, this.Radiant, targetMask);
        List<GameObject> returnObj = new List<GameObject>();
        if (obj.Length > 0)
        {
            foreach (Collider tarObj in obj)
            {
                Vector3 Objdirection = tarObj.transform.position - objView.transform.position;
                Objdirection.Normalize();
                if (Vector3.Angle(objView.transform.forward, Objdirection) < AngelInDegree / 2)
                {
                    if (Physics.Raycast(objView.transform.position, tarObj.transform.position, out RaycastHit hit, 1000))
                    {
                        if (hit.collider.gameObject == tarObj.gameObject)
                        {
                            returnObj.Add(tarObj.gameObject);
                        }
                    }
                }
            }
        }
        return returnObj;
    }
    public List<GameObject> FindMutipleOnjectInArea(LayerMask targetMask)
    {
        List<GameObject> returnObj = new List<GameObject>();
        Collider[] obj = Physics.OverlapSphere(objView.transform.position, this.Radiant, targetMask);
        if (obj.Length > 0)
        {
            foreach(Collider tarObj in obj) 
            {
                returnObj.Add(tarObj.gameObject);
            }
        }
        return returnObj;
    }
    public List<GameObject> FindMutipleOnjectInArea(LayerMask targetMask,float Radians)
    {
        List<GameObject> returnObj = new List<GameObject>();
        Collider[] obj = Physics.OverlapSphere(objView.transform.position,Radians, targetMask);
        if (obj.Length > 0)
        {
            foreach (Collider tarObj in obj)
            {
                returnObj.Add(tarObj.gameObject);
            }
        }
        return returnObj;
    }
}