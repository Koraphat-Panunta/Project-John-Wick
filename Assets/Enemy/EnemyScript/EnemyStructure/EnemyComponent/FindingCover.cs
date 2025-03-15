using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public abstract class FindingCover
{
    protected ICoverUseable coverUser;
    protected LayerMask coverLayer;
    protected IFindingTarget hunter;
    public FindingCover(ICoverUseable coverUser, IFindingTarget hunter)
    {
        this.coverUser = coverUser;
        this.coverLayer = LayerMask.GetMask("Cover");
        this.hunter = hunter;
    }
    public abstract bool FindCoverInRaduis(float raduis, out CoverPoint coverPoint);
    
       
}
public class EnemyFindCover : FindingCover
{
    private Enemy enemy;
    public EnemyFindCover(ICoverUseable coverUser, IFindingTarget hunter,Enemy enemy) : base(coverUser, hunter)
    {
        this.enemy = enemy;
    }

    public override bool FindCoverInRaduis(float raduis, out CoverPoint coverPoint)
    {
        coverPoint = null;
        List<CoverPoint> _coverPoint = new List<CoverPoint>();

        Collider[] obj = Physics.OverlapSphere(coverUser.userCover.transform.position, raduis);

        if (obj.Length <= 0)
            return false;

        foreach (Collider collider in obj)
        {

            if (collider.TryGetComponent<CoverPoint>(out CoverPoint thisCoverPoint) == false)
                continue;

            if (thisCoverPoint.coverUser != null)
                continue;

            switch (thisCoverPoint)
            {

                case CoverPointTallSingleSide coverPointTall_Single:
                    {
                        if (coverPointTall_Single.CheckingTargetInCoverView(
                            coverUser,
                            enemy.targetSpoterMask,
                            coverPointTall_Single.peekPos,
                            out GameObject target)
                            )
                            _coverPoint.Add(coverPointTall_Single);

                    };
                    break;
                case CoverPointTallDoubleSide coverPointTall_Double:
                    {
                        if (coverPointTall_Double.CheckingTargetInCoverView(
                            coverUser,
                            enemy.targetSpoterMask,
                            coverPointTall_Double.peekPosR,
                            out GameObject targetR)
                            )
                            _coverPoint.Add(coverPointTall_Double);

                        else if (coverPointTall_Double.CheckingTargetInCoverView(
                            coverUser,
                            enemy.targetSpoterMask,
                            coverPointTall_Double.peekPosL,
                            out GameObject targetL)
                            )
                            _coverPoint.Add(coverPointTall_Double);
                    }
                    break;
                case CoverPointShort coverPointShort:
                    {
                        if (coverPointShort.CheckingTargetInCoverView(
                            coverUser,
                            enemy.targetSpoterMask,
                            coverPointShort.peekPos,
                            out GameObject target)
                            )
                            _coverPoint.Add(coverPointShort);
                    };
                    break;

            }
        }

        if (_coverPoint.Count <= 0)
            return false;

        for (int i = 0; i < _coverPoint.Count; i++)
        {
            Vector3 dir_Target_To_Cover = (_coverPoint[i].coverPos.position - enemy.targetKnewPos).normalized;
            Vector3 dir_Target_To_User = (coverUser.userCover.transform.position - enemy.targetKnewPos).normalized;

            if (Vector3.Dot(dir_Target_To_User, dir_Target_To_Cover) < 0.5f)
                _coverPoint.RemoveAt(i);

        }

        if (_coverPoint.Count <= 0)
            return false;

        for (int i = 0; i < _coverPoint.Count; i++)
        {

            if (i == 0)
            {
                coverPoint = _coverPoint[0];
                continue;
            }

            if (Vector3.Distance(coverPoint.coverPos.position, coverUser.userCover.transform.position) >
                Vector3.Distance(_coverPoint[i].coverPos.position, coverUser.userCover.transform.position))
            {
                coverPoint = _coverPoint[i];
            }
        }
        return true;
    }
}
