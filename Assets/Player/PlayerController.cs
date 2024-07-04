using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public InputAction Move;
    public Vector2 Movement;

    private void OnEnable()
    {
        Move.Enable();
    }
    private void OnDisable()
    {
        Move.Disable();
    }
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Move.ReadValue<Vector2>());
        Movement = Move.ReadValue<Vector2>();
    }
  
}
