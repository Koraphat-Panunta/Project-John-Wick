using UnityEngine;

public class PlayerDetectionPoint : MonoBehaviour,I_EnemyAITargeted
{
    [SerializeField] private Player player;
    public Character selfEnemyAIBeenTargeted => player.selfEnemyAIBeenTargeted;
}
