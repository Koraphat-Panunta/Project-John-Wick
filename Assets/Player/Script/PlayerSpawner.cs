using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PlayerSpawner : MonoBehaviour
{
    public Player playerSpawnObject;
    public CrosshairController playerCrosshairController;
    public List<IObserverPlayerSpawner> observerPlayerSpawners = new List<IObserverPlayerSpawner>();
    private Player player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void Awake()
    {
        playerCrosshairController = Resources.FindObjectsOfTypeAll<CrosshairController>().FirstOrDefault();
        player = Instantiate(playerSpawnObject, transform.position, transform.rotation);
        player.crosshairController = playerCrosshairController;
        player.gameObject.SetActive(false);
    }
    public void Start()
    {
       
    }
    public void SpawnPlayer()
    {
        player.gameObject.SetActive(true);
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
