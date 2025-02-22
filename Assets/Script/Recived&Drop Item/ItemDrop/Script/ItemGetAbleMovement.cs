using UnityEngine;

public class ItemGetAbleMovement : MonoBehaviour
{
    [Header("Spin Settings")]
    public Vector3 rotationSpeed = new Vector3(0, 50, 0); // Degrees per second

    [Header("Float Settings")]
    public float floatAmplitude = 0.5f; // How high it moves
    public float floatSpeed = 2f; // How fast it moves up and down

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.localPosition; // Store the original position
    }

    void Update()
    {
        // Rotate the object
        transform.Rotate(rotationSpeed * Time.deltaTime);

        // Move up and down using a sine wave
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.localPosition = startPosition + new Vector3(0, yOffset, 0);
    }
}
