using UnityEngine;

public class User : MonoBehaviour,IInitializedAble
{
    public UserInput userInput { get; protected set; }
    [SerializeField] private Player player;

    
    public void EnableInput()
    {
        if(userInput == null)
            userInput = new UserInput();

        userInput.Enable();
    }
    public void DisableInput()
    {
        userInput.Disable();
    }

  
    private void OnDisable()
    {
        DisableInput();
    }
    private void OnValidate()
    {
        player = FindAnyObjectByType<Player>();
    }

    public void Initialized()
    {    
        userInput = new UserInput();
        EnableInput();
    }

}
