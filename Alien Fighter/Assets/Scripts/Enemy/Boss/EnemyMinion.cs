using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMinion : EnemyController
{
    public Vector3 PosToMoveOn { get; set; }

    private void OnEnable()
    {
        if (isStart)
        {
            StartSettings();
            return;
        }

        SetUp();
    }

    public void LocalStartSettings(GameObject parent)
    {
        Pool = parent;
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

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 dir = PosToMoveOn - transform.position;
        dir.z = 0;

        if (dir.magnitude > 0.5f)
        {
            transform.position += dir.normalized * 0.5f;
        }
        
    }
}
