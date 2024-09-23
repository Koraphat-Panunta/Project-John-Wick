using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1 : LevelManager
{
    [SerializeField] protected List<Character> targetEliminate;
    public Elimination elimination;
    public Score score;
    protected override void Start()
    {
        //foreach (Character target in Resources.FindObjectsOfTypeAll(typeof(Enemy)) as Enemy[]) 
        //{
        //    targetEliminate.Add(target);
        //}
        elimination = new Elimination(targetEliminate,this);
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
    private void OnEnable()
    {
        if (score == null)
        {
            score = new Score();
        }
        foreach (Enemy enemy in Resources.FindObjectsOfTypeAll(typeof(Enemy)) as Enemy[])
        {
            Debug.Log(enemy.name);
            enemy.AddObserver(score);
        }

    }
    private void OnDisable()
    {
        foreach (Enemy enemy in Resources.FindObjectsOfTypeAll(typeof(Enemy)) as Enemy[])
        {
            enemy.RemoveObserver(score);
        }
    }
}
