using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnemyMiniFlinch 
{
     Coroutine currentCoroutine;
    private Enemy enemy;
    private MultiRotationConstraint rotationConstraintFlinch;
    public EnemyMiniFlinch(Enemy enemy)
    {
        this.enemy = enemy;
        this.rotationConstraintFlinch = enemy.rotationConstraint;
        this.rotationConstraintFlinch.weight = 0;
    }
    public void TriggerFlich()
    {
        // If there is an already running coroutine, stop it
        if (currentCoroutine != null)
        {
            this.enemy.StopCoroutine(currentCoroutine);
        }

        // Start the new coroutine and store the reference
        currentCoroutine = this.enemy.StartCoroutine(ExampleCoroutine());
    }

    IEnumerator ExampleCoroutine()
    {
       rotationConstraintFlinch.weight = 1.0f;
       while(rotationConstraintFlinch.weight > 0)
        {
            rotationConstraintFlinch.weight -= Time.deltaTime*1.8f;
            yield return null;
        }
    }
}
