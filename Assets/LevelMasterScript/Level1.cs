using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1 : LevelManager
{
    [SerializeField] protected List<Character> targetEliminate;
    public Elimination elimination;
    protected override void Start()
    {
        elimination = new Elimination(targetEliminate);
        base.levelObjective.Add(elimination);
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
    }
    protected override void LevelClear()
    {
        base.LevelClear();
    }
    protected override void ObjectiveUpdate()
    {
        base.ObjectiveUpdate();
    }
}
