using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    public float swayAmount = 0.02f;
    public float swaySpeed = 2f;

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * swayAmount;
        float mouseY = Input.GetAxis("Mouse Y") * swayAmount;

        Vector3 finalPosition = new Vector3(initialPosition.x + mouseX, initialPosition.y + mouseY, initialPosition.z);
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition, Time.deltaTime * swaySpeed);
    }
}
