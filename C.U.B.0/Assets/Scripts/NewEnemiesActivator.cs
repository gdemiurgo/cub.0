using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemiesActivator : MonoBehaviour
{
    [SerializeField] private GameController gameController;

    [SerializeField] Enemy[] enemiesToActivate;
    [SerializeField] Enemy[] enemiesDefeated;
    [SerializeField] SmoothMover smoothMover;

    [SerializeField] Animator animator;
    [SerializeField] string onWayAnimation;

    [SerializeField] bool activateOnEnemiesDefeated;
    [SerializeField] bool activateOnUnlocks;

    private bool allUnlocked;
    private bool allEnemiesDefeated = false;

    void Update()
    {
        if(activateOnUnlocks && !allUnlocked)
        {
            ChecAllUnlocked();
        }
        else if (activateOnEnemiesDefeated && !allEnemiesDefeated)
        {
            CheckEnemiesDefeated();
        }
    }

    private void CheckEnemiesDefeated()
    {
        bool allDead = false;

        foreach(Enemy enemy in enemiesDefeated)
        {
            allDead = enemy.dead;
        }

        if(allDead)
        {
            foreach(Enemy enemy in enemiesToActivate)
            {
                enemy.gameObject.SetActive(true);
                enemy.IniEnemy();
            }

            smoothMover.StartMoving();
            allEnemiesDefeated = true;
        }
    }

    private void ChecAllUnlocked()
    {
        if(gameController.AllUnlocked())
        {
            //smoothMover.StartMoving();

            animator.SetTrigger(onWayAnimation);

            foreach (Enemy enemy in enemiesToActivate)
            {
                enemy.gameObject.SetActive(true);
                enemy.IniEnemy();
            }

            allUnlocked = true;
        }
    }
}

