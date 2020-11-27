using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy00 : EnemyController
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

    private void Update()
    {
        MakeDamage();
        SizeUp();
    }
}
