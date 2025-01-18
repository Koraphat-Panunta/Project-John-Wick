using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FieldOfView
{
    private float Radiant;
    private float AngelInDegree;
    private Transform objView;
    private LayerMask defualtLayerMask;
    public FieldOfView(float radiant, float angelInDegree, Transform objView)
    {
        Radiant = radiant;
        AngelInDegree = angelInDegree;
        this.objView = objView;
        defualtLayerMask = LayerMask.GetMask("Default");
    }
    public GameObject FindSingleObjectInView(LayerMask targetMask)
    {
        Collider[] obj = Physics.OverlapSphere(objView.transform.position, this.Radiant, targetMask);
        GameObject returnObj = null;
        if (obj[0] != null)
        {
            Vector3 Objdirection = obj[0].transform.position - objView.transform.position;
            Objdirection.Normalize();
            if (Vector3.Angle(objView.transform.forward, Objdirection) < AngelInDegree / 2)
            {

                if (Physics.Raycast(objView.transform.position, (obj[0].transform.position-objView.transform.position).normalized, out RaycastHit hit, 1000,defualtLayerMask+targetMask))
                {                   
                    if (hit.collider.gameObject.layer == obj[0].gameObject.layer)
                    {
                        returnObj = obj[0].gameObject;
                    }
                }
            }
        }
        return returnObj;
    }

    public bool FindSingleObjectInView(LayerMask targetMask,Vector3 offsetView,out GameObject targetObj)
    {
        Collider[] obj = Physics.OverlapSphere(objView.transform.position, this.Radiant, targetMask);
        targetObj = null;

        if(obj.Length <=0)
            return false;

        Vector3 Objdirection = obj[0].transform.position - objView.transform.position;
        Objdirection.Normalize();
        if (Vector3.Angle(objView.transform.forward, Objdirection) >= AngelInDegree / 2)
            return false;

        if (Physics.Raycast(objView.transform.position, (obj[0].transform.position - objView.transform.position).normalized, out RaycastHit hit, 1000, defualtLayerMask + targetMask))
        {
            if (hit.collider.gameObject.layer == obj[0].gameObject.layer)
            {
                targetObj = obj[0].gameObject;
                return true;
            }
        }
        return false;
    }
    public bool FindSingleObjectInView(LayerMask targetMask, out GameObject targetObj)
    {
        Collider[] obj = Physics.OverlapSphere(objView.transform.position, this.Radiant, targetMask);
        targetObj = null;

        if (obj.Length <= 0)
            return false;

        Vector3 Objdirection = obj[0].transform.position - objView.transform.position;
        Objdirection.Normalize();
        if (Vector3.Angle(objView.transform.forward, Objdirection) >= AngelInDegree / 2)
            return false;

        if (Physics.Raycast(objView.transform.position, (obj[0].transform.position - objView.transform.position).normalized, out RaycastHit hit, 1000, defualtLayerMask + targetMask))
        {
            if (hit.collider.gameObject.layer == obj[0].gameObject.layer)
            {
                targetObj = obj[0].gameObject;
                return true;
            }
        }
        return false;
    }

    public bool FindSingleObjectInView(LayerMask targetMask,Vector3 lookDir,float angleInDegrees, out GameObject targetObj)
    {
        Collider[] obj = Physics.OverlapSphere(objView.transform.position, this.Radiant, targetMask);
        targetObj = null;

        if (obj.Length <= 0)
            return false;

        Vector3 Objdirection = obj[0].transform.position - objView.transform.position;
        Objdirection.Normalize();
        if (Vector3.Angle(lookDir, Objdirection) >= angleInDegrees / 2)
            return false;

        if (Physics.Raycast(objView.transform.position, (obj[0].transform.position - objView.transform.position).normalized, out RaycastHit hit, 1000, defualtLayerMask + targetMask))
        {
            if (hit.collider.gameObject.layer == obj[0].gameObject.layer)
            {
                targetObj = obj[0].gameObject;
                return true;
            }
        }
        return false;
    }


    public GameObject FindSingleObjectInView(LayerMask targetMask,Vector3 offsetView)
    {
        Collider[] obj = Physics.OverlapSphere(objView.transform.position, this.Radiant, targetMask);
        GameObject returnObj = null;
        if(obj.Length <= 0)
        {
            return null;
        }
        if (obj[0] != null)
        {
            Vector3 Objdirection = obj[0].transform.position - objView.transform.position;
            Objdirection.Normalize();
            if (Vector3.Angle(objView.transform.forward, Objdirection) < AngelInDegree / 2)
            {
                if (Physics.Raycast(objView.transform.position + offsetView, (obj[0].transform.position - (objView.transform.position+ offsetView)).normalized, out RaycastHit hit, 1000, defualtLayerMask + targetMask))
                {
                    if (hit.collider.gameObject.layer == obj[0].gameObject.layer)
                    {
                        returnObj = obj[0].gameObject;
                    }
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
                    if (Physics.Raycast(objView.transform.position, tarObj.transform.position, out RaycastHit hit, 1000, defualtLayerMask + targetMask))
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
