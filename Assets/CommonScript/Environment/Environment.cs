using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;

public class Environment : MonoBehaviour,IObserverPlayer
{
    List<IEnvironmentAware> environmentAwareir = new List<IEnvironmentAware>();
    
    private void Start()
    {
        Player player = FindAnyObjectByType<Player>();
        if (player != null)
        {
            player.AddObserver(this);
        }
    }
    public void OnNotify(Player player, SubjectPlayer.PlayerAction playerAction)
    {
       if(playerAction == SubjectPlayer.PlayerAction.Firing)
        {
            Notify_Listen(player.gameObject, EnvironmentType.Sound);
        }
    }
    public void Add_Listener(IEnvironmentAware environmentAware)
    {
        environmentAwareir.Add(environmentAware);
    }
    public void Remove_listener(IEnvironmentAware environmentAware)
    {
        environmentAwareir.Remove(environmentAware);
    }
    public void Notify_Listen(GameObject source,EnvironmentType environmentType)
    {
        if (environmentAwareir.Count > 0)
        {
            foreach (IEnvironmentAware environmentAwareir in environmentAwareir)
            {
                environmentAwareir.OnAware(source, environmentType);
            }
        }
    }

}
