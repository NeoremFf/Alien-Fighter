using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy02 : EnemyController
{
    [SerializeField] private float speed;

    private bool canDamage = true;

    private void OnEnable()
    {
        if (isStart)
        {
            StartSettings();
            return;
        }

        SetUp();
        canDamage = true;
    }

    private void OnMouseDown()
    {
        GetDamage();
    }

    private void Update()
    {
        Move();
        SizeUp();
    }

    private void Move()
    {
        Vector3 dir = _hero.gameObject.transform.position - transform.position;

        if (dir.magnitude < 0.8f &&
            canDamage)
        {
            canDamage = false;
            MakeDamage();
            return;
        }

        dir.z = 0;
        transform.position += dir.normalized  * Time.deltaTime * speed;
        transform.right = dir;
    }
}
