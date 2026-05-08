using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack_Defensive_DetectAttacker : MonoBehaviour, IInitializedAble
{
    [SerializeField] private Transform CastTransform;
    public Transform _castTransform { get => this.CastTransform; set => this.CastTransform = value; }

    [Range(0, 360)]
    [SerializeField] private float LimitAimAngleDegrees = 120f;
    public float _limitAimAngleDegrees { get => this.LimitAimAngleDegrees; set => this.LimitAimAngleDegrees = value; }

    [Range(0, 10)]
    [SerializeField] private float Sphere_Radius_Detection = 1.5f;
    public float _sphere_Radius_Detection { get => this.Sphere_Radius_Detection; set => this.Sphere_Radius_Detection = value; }

    [Range(0, 10)]
    [SerializeField] private float Sphere_Distance_Detection = 2f;
    public float _sphere_Distance_Detection { get => this.Sphere_Distance_Detection; set => this.Sphere_Distance_Detection = value; }

    [SerializeField] public LayerMask _layerAttacker;

    [SerializeField] private MeleeAttackingPhase[] _notifyPhases = new MeleeAttackingPhase[]
    {
        MeleeAttackingPhase.Anticipate,
        MeleeAttackingPhase.PreAttack,
    };

    [SerializeField] private bool EnableDebug;

    private IDefendMeleeAttackAble defender;
    private readonly Dictionary<IMeleeAttackerAble, MeleeAttackingPhase> trackedAttackers = new Dictionary<IMeleeAttackerAble, MeleeAttackingPhase>();
    private readonly List<IMeleeAttackerAble> seenThisFrame = new List<IMeleeAttackerAble>();
    private readonly List<IMeleeAttackerAble> toRemove = new List<IMeleeAttackerAble>();

    public void Initialized()
    {
        this.defender = GetComponent<IDefendMeleeAttackAble>();
    }

    private void Update()
    {
        if (this.defender == null)
            return;

        if (this.defender._character != null && this.defender._character.isDead)
        {
            ClearAllTracked();
            return;
        }

        seenThisFrame.Clear();

        Vector3 castPos = this.CastTransform != null ? this.CastTransform.position : transform.position;
        float overlapRadius = this.Sphere_Radius_Detection + this.Sphere_Distance_Detection;

        Collider[] colliders = Physics.OverlapSphere(castPos, overlapRadius, this._layerAttacker, QueryTriggerInteraction.Collide);

        foreach (Collider col in colliders)
        {
            if (col.TryGetComponent<IMeleeAttackerAble>(out IMeleeAttackerAble attacker) == false)
                continue;

            if (attacker._character == null || attacker._character == this.defender._character)
                continue;

            if (attacker._character.isDead)
                continue;

            Vector3 toAttacker = attacker._attackerTransform.position - this.defender._defenderTransform.position;
            if (toAttacker.sqrMagnitude > 0.0001f)
            {
                float angle = Vector3.Angle(this.defender._defenderTransform.forward, toAttacker);
                if (angle > this.LimitAimAngleDegrees)
                    continue;
            }

            MeleeAttackingPhase phase = attacker._curAttackPhase;
            if (IsNotifyPhase(phase) == false)
                continue;

            seenThisFrame.Add(attacker);

            if (this.trackedAttackers.ContainsKey(attacker) == false)
            {
                this.trackedAttackers.Add(attacker, phase);
                this.defender.OnIncomingMeleeAttack(attacker);
            }
            else
            {
                this.trackedAttackers[attacker] = phase;
            }
        }

        toRemove.Clear();
        foreach (KeyValuePair<IMeleeAttackerAble, MeleeAttackingPhase> kvp in this.trackedAttackers)
        {
            if (seenThisFrame.Contains(kvp.Key) == false)
                toRemove.Add(kvp.Key);
        }

        foreach (IMeleeAttackerAble attacker in toRemove)
        {
            this.trackedAttackers.Remove(attacker);
            this.defender.OnIncomingMeleeAttackEnded(attacker);
        }
    }

    private bool IsNotifyPhase(MeleeAttackingPhase phase)
    {
        for (int i = 0; i < this._notifyPhases.Length; i++)
        {
            if (this._notifyPhases[i] == phase)
                return true;
        }
        return false;
    }

    private void ClearAllTracked()
    {
        if (this.trackedAttackers.Count <= 0)
            return;

        toRemove.Clear();
        foreach (KeyValuePair<IMeleeAttackerAble, MeleeAttackingPhase> kvp in this.trackedAttackers)
            toRemove.Add(kvp.Key);

        this.trackedAttackers.Clear();

        foreach (IMeleeAttackerAble attacker in toRemove)
            this.defender.OnIncomingMeleeAttackEnded(attacker);
    }

    private void OnDrawGizmos()
    {
        if (this.EnableDebug == false)
            return;

        if (this.CastTransform == null)
            return;

        Vector3 origin = this.CastTransform.position;
        Vector3 forward = transform.forward;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(origin, this.Sphere_Radius_Detection + this.Sphere_Distance_Detection);

        Gizmos.color = Color.yellow;
        Vector3 leftEdge = Quaternion.Euler(0, -this.LimitAimAngleDegrees, 0) * forward;
        Vector3 rightEdge = Quaternion.Euler(0, this.LimitAimAngleDegrees, 0) * forward;
        Gizmos.DrawLine(origin, origin + leftEdge * (this.Sphere_Radius_Detection + this.Sphere_Distance_Detection));
        Gizmos.DrawLine(origin, origin + rightEdge * (this.Sphere_Radius_Detection + this.Sphere_Distance_Detection));
    }
}
