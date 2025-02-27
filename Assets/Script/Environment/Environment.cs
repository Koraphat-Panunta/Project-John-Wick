using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.AI.Navigation;
using UnityEngine;


public class Environment : MonoBehaviour,IObserverPlayer,IObserverPlayerSpawner
{
    List<IEnvironmentAware> environmentAwareir = new List<IEnvironmentAware>();
    [SerializeField] private PlayerSpawner playerSpawner;
    private void Awake()
    {
        playerSpawner = FindAnyObjectByType<PlayerSpawner>();
        playerSpawner.AddObserverPlayerSpawner(this);
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

    public void OnNotify(Player player)
    {
       
    }

    public void GetNotify(Player player)
    {
        player.AddObserver(this);
    }
}
