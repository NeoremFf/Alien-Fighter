using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy04 : EnemyController
{
    private void OnEnable()
    {
        if (isStart)
        {
            StartSettings();
            return;
        }
        SetUp();
    }

    private void OnMouseDown()
    {
        GetDamage();
    }

    protected override void GetDamage()
    {
        Destroy(gameObject);
        _hero.GetDamage(damage);
        sound.PlayEnemyMakesDamage();
    }

    private void Update()
    {
        MakeDamage();
        SizeUp();
    }

    protected override void MakeDamage()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        { 
           Dead();
        }
    }
}
