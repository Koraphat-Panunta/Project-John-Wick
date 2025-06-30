using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObjectToward 
{
   
    public void Performd()
    {
        
    }
    public void RotateToward(Vector3 direction,GameObject _rotObject,float rotationSpeed_NoNeedDeltaTime)
    {
        // Ensure the direction is normalized
        direction.Normalize();

        // Flatten the direction vector to the XZ plane to only rotate around the Y axis
        direction.y = 0;

        // Check if the direction is not zero to avoid setting a NaN rotation
        if (direction != Vector3.zero)
        {
            // Calculate the target rotation based on the direction
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Smoothly rotate towards the target rotation
            _rotObject.transform.rotation = Quaternion.Slerp(_rotObject.transform.rotation, targetRotation, rotationSpeed_NoNeedDeltaTime * Time.deltaTime);
        }
    }
    public Quaternion RotateToward(Vector3 direction, Transform _rotObject, float rotationSpeed_NoNeedDeltaTime)
    {

        direction.Normalize();

        direction.y = 0;
 
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        return Quaternion.Slerp(_rotObject.rotation, targetRotation, rotationSpeed_NoNeedDeltaTime * Time.deltaTime);
        
    }
    public void RotateTowardsObjectPos(Vector3 targetPos, GameObject _rotObject, float rotationSpeed_NoNeedDeltaTime)
    {
        // Ensure the direction is normalized
        Vector3 direction = targetPos - _rotObject.transform.position;
        direction.Normalize();
        // Flatten the direction vector to the XZ plane to only rotate around the Y axis
        direction.y = 0;
        // Check if the direction is not zero to avoid setting a NaN rotation
        if (direction != Vector3.zero)
        {
            // Calculate the target rotation based on the direction
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Smoothly rotate towards the target rotation
            _rotObject.transform.rotation = Quaternion.Slerp(_rotObject.transform.rotation, targetRotation, rotationSpeed_NoNeedDeltaTime * Time.deltaTime);
        }
    }
    public void RotateTowards(GameObject direction, GameObject _rotObject, float rotationSpeed_NoNeedDeltaTime)
    {
        // Ensure the direction is normalized
        Vector3 Dir = direction.transform.position - _rotObject.transform.position;
        Dir.Normalize();

        // Flatten the direction vector to the XZ plane to only rotate around the Y axis
        Dir.y = 0;

        // Check if the direction is not zero to avoid setting a NaN rotation
        if (Dir != Vector3.zero)
        {
            // Calculate the target rotation based on the direction
            Quaternion targetRotation = Quaternion.LookRotation(Dir);

            // Smoothly rotate towards the target rotation
            _rotObject.transform.rotation = Quaternion.Slerp(_rotObject.transform.rotation, targetRotation, rotationSpeed_NoNeedDeltaTime * Time.deltaTime);
        }
    }
    public void RotateTowardsObject(GameObject target,GameObject rotObject,float rotSpeed)
    {
        Vector3 dir = (target.transform.position - rotObject.transform.position).normalized;
        RotateToward(dir, rotObject, rotSpeed);
    }
}
