using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObjectToward : IUtilityMethod
{
    public RotateObjectToward() 
    {
        
    }
    public void Performd()
    {
        
    }
    public void RotateTowards(Vector3 direction,GameObject _rotObject,float rotationSpeed)
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
            _rotObject.transform.rotation = Quaternion.Slerp(_rotObject.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}