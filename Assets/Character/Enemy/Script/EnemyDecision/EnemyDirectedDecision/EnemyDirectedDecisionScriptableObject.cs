using UnityEngine;
[CreateAssetMenu(fileName = "EnemyDirectedDecisionSCRP", menuName = "ScriptableObjects/Enemy/EnemyDirectedDecisionScriptableObject")]
public class EnemyDirectedDecisionScriptableObject : ScriptableObject
{
    [System.Serializable]
    public struct PhaseTimerParameter
    {
        public float duration;
        public float[] phaseThresholds;
    }

    [SerializeField] public EnemyDecisionContextScriptableObject enemyDecisionContextScriptableObject;
    [SerializeField] public PhaseTimerParameter camperPhaseTimer;
    [SerializeField] public PhaseTimerParameter engagingPhaseTimer;
}
