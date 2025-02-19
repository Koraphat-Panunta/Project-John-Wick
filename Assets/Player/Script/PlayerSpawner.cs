using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PlayerSpawner : MonoBehaviour
{
    public Player playerSpawnObject;
    public CrosshairController playerCrosshairController;
    public List<IObserverPlayerSpawner> observerPlayerSpawners = new List<IObserverPlayerSpawner>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void Awake()
    {
        playerCrosshairController = Resources.FindObjectsOfTypeAll<CrosshairController>().FirstOrDefault();
    }
    public void SpawnPlayer()
    {
        Player player = Instantiate(playerSpawnObject,transform.position,transform.rotation);
        player.crosshairController = playerCrosshairController;
        foreach (IObserverPlayerSpawner observer in observerPlayerSpawners)
        {
            observer.GetNotify(player);
        }
        Destroy(gameObject);
    }
    public void AddObserverPlayerSpawner(IObserverPlayerSpawner observerPlayerSpawner)
    {
        this.observerPlayerSpawners.Add(observerPlayerSpawner);
    }
}

public interface IObserverPlayerSpawner
{
    public void GetNotify(Player player);
}
