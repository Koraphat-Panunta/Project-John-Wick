using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnemyMiniFlinch 
{
    Coroutine currentCoroutine;
    private Enemy enemy;
    private MultiRotationConstraint rotationConstraintFlinch;
    private float flinchRate;
    public EnemyMiniFlinch(Enemy enemy)
    {
        this.enemy = enemy;
        this.rotationConstraintFlinch = enemy.rotationConstraint;
        this.rotationConstraintFlinch.weight = 0;
        flinchRate = 0;
    }
    public void TriggerFlich()
    {
        Debug.Log("Trigger Flinch");
        if (currentCoroutine != null)
        {
            this.enemy.StopCoroutine(currentCoroutine);
            Debug.Log("Cancel Weight");
        }
        currentCoroutine = this.enemy.StartCoroutine(ExampleCoroutine());
    }

    IEnumerator ExampleCoroutine()
    {
        flinchRate = 1;

        while (flinchRate > 0)
        {
            rotationConstraintFlinch.weight = flinchRate;
            flinchRate -= Time.deltaTime*2;
            yield return null;
        }
        
    }
}
