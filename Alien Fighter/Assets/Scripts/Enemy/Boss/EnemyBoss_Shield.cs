using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss_Shield : EnemyController
{
    private EnemyBoss _mainCoreScript;

    private void OnEnable()
    {
        if (isStart)
        {
            StartSettings();
            return;
        }

        SetUp();
        _mainCoreScript.AddShield();
    }

    public void LocalStartSettings(GameObject parent)
    {
        Pool = parent;

        _mainCoreScript = parent.GetComponent<EnemyBoss>();
    }

    protected override void GetDamage()
    {
        hp--;
        if (hp <= 0)
        {
            _mainCoreScript.RemoveShield();
            Dead();
        }
    }

    private void OnMouseDown()
    {
        GetDamage();
    }

    private void Update()
    {
        SizeUp();
    }
}
