using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/Enemy/EnemyData")]
public class EnemyDataScriptableObject : DataScriptableObject
{
    [SerializeField] public Enemy enemyPrefab;
}
