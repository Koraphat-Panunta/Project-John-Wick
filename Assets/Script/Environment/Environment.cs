using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.AI.Navigation;
using UnityEngine;


public class Environment : MonoBehaviour,IObserverPlayer
{
    List<IEnvironmentAware> environmentAwareir = new List<IEnvironmentAware>();
    [SerializeField] private Player player;
    private void Awake()
    {
        
    }
    private void OnEnable()
    {
        player.AddObserver(this);
    }
    private void OnDisable()
    {
        player.RemoveObserver(this);
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

 
}
