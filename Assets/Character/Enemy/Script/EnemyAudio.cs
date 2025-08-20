using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio : MonoBehaviour,IObserverEnemy
{
    // Start is called before the first frame update
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource walkSource;
    [SerializeField] private AudioClip hit;
    [SerializeField] private AudioClip dead;
    [SerializeField] private AudioClip footStep;
    [SerializeField] private Enemy enemy;
    [SerializeField] private Animator animator;
    public void Notify(Enemy enemy, SubjectEnemy.EnemyEvent enemyEvent)
    {
       
    }
    public void Notify<T>(Enemy enemy, T node) where T : INode
    {
        
    }
    bool isdead = false;
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        animator = enemy.animator;
        GetComponent<Enemy>().AddObserver(this);
    }
   
    private void OnDisable()
    {
        GetComponent<Enemy>().RemoveObserver(this);
    }
    private void Update()
    {
        PlayVolumeMove();
    }
    float footStepTiming = 0;
    private void PlayVolumeMove()
    {
        EnemyStateLeafNode enemyState = (enemy.enemyStateManagerNode as INodeManager).GetCurNodeLeaf() as EnemyStateLeafNode;
        if (enemyState is EnemyStandMoveStateNodeLeaf)
        {
            float timingRate = 1.2f;
            footStepTiming += Time.deltaTime * timingRate;
            if (footStepTiming >= 1)
            {
                walkSource.clip = footStep;
                walkSource.Play();
                footStepTiming = 0;
            }
        }
        else if (enemyState is EnemySprintStateNodeLeaf)
        {
            float timingRate = 2.4f;
            footStepTiming += Time.deltaTime * timingRate;
            if (footStepTiming >= 1)
            {
                walkSource.clip = footStep;
                walkSource.Play();
                footStepTiming = 0;
            }
        }
        else
        {
            footStepTiming = 0;
        }
    }
}
