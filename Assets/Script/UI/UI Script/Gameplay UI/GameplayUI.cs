using UnityEngine;

public abstract class GameplayUI : MonoBehaviour
{
    public bool isEnable { get; set; }
    public virtual void EnableUI()
    {
        isEnable = true;
    }
    public virtual void DisableUI()
    {
        isEnable = false;
    }
}
